using System;
using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
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

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        
        public GameObject HeroGameObject { get; private set; }
        
        public GameFactory(IAssetProvider assets, IStaticDataService staticData)
        {
            _assets = assets;
            _staticData = staticData;
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

        public GameObject CreateHud() => 
            InstantiateRegistred(AssetPath.HudPath);

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
            
            return monster;
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