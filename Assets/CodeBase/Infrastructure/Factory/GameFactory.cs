using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private IUncollectedLootChecker _uncollectedLootChecker;
        private IWindowService _windowService;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        
        public GameObject HeroGameObject { get; private set; }
        
        public GameFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService, IUncollectedLootChecker uncollectedLootChecker, IWindowService windowService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _uncollectedLootChecker = uncollectedLootChecker;
            _windowService = windowService;
        }

        public GameObject CreateHero(Vector3 at)
        {
            HeroGameObject = InstantiateRegistred(AssetPath.HeroPath, at);
            return HeroGameObject;
        }

        private GameObject InstantiateRegistred(string prefab, Vector3 at = default)
        {
            var gameObject = _assets.InstantiatePrefab(prefab, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        public GameObject CreateHud()
        {
            var hud = InstantiateRegistred(AssetPath.HudPath);

            foreach (var openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>()) 
                openWindowButton.Init(_windowService);

            hud.GetComponentInChildren<LootCounter>()
                .Init(_progressService.Progress.WorldData);
            
            return hud;
        }
        
        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
            _assets.CleanUp();
        }

        public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            var monsterData = _staticData.ForMonster(typeId);

            var prefab = await _assets.Load<GameObject>(monsterData.PrefabReference);
            var monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
            
            var health = monster.GetComponent<IHealth>();
            health.Current = monsterData.Hp;
            health.Max = monsterData.Hp;
            
            monster.GetComponent<ActorUI>().Init(health);
            monster.GetComponent<AgentMoveToPlayer>().Init(HeroGameObject.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;
            
            var attack = monster.GetComponent<Attack>();
            attack.Init(HeroGameObject.transform, monsterData.Cleavage, damage: monsterData.Damage);

            if (monster.TryGetComponent<RotateToHero>(out var rotateToHero))
                rotateToHero.Init(HeroGameObject.transform);
            
            var loot = monster.GetComponentInChildren<LootSpawner>();
            loot.Init(this, _uncollectedLootChecker);
            loot.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
            
            return monster;
        }

        public void CreateSpawner(Vector3 position, string spawnerId, MonsterTypeId spawnerMonsterTypeId)
        {
            var spawner = InstantiateRegistred(AssetPath.Spawner, position)
                .GetComponent<SpawnPoint>();
            spawner.Init(this);
            
            spawner.Id = spawnerId;
            spawner.MonsterTypeId = spawnerMonsterTypeId;
        }

        public LootPiece CreateLoot()
        {
            var lootPiece = InstantiateRegistred(AssetPath.Loot)
                .GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.WorldData);
            
            return lootPiece;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>()) 
                Register(progressReader);
        }

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter); 
                
            ProgressReaders.Add(progressReader);
        }
    }
}