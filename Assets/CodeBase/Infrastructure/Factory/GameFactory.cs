using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.UI;
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

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        
        public GameObject HeroGameObject { get; private set; }
        
        public GameFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService, IUncollectedLootChecker uncollectedLootChecker)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _uncollectedLootChecker = uncollectedLootChecker;
        }

        public GameObject CreateHero(InitialPoint at)
        {
            HeroGameObject = InstantiateRegistred(AssetPath.HeroPath, at.transform.position);
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

            hud.GetComponentInChildren<LootCounter>()
                .Init(_progressService.Progress.WorldData);
            
            return hud;
        }
        
        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            var monsterData = _staticData.ForMonster(typeId);
            var monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

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