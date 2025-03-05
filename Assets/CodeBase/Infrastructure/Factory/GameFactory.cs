using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        
        public GameObject HeroGameObject { get; private set; }
        
        public event Action HeroCreated;

        public GameFactory(IAssetProvider assets)
        {
            _assets = assets;
        }
        public GameObject CreateHero(InitialPoint at)
        {
            HeroGameObject = InstantiateRegistred(AssetPath.HeroPath, at.transform.position);
            HeroCreated?.Invoke();
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
        
        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>()) 
                Registrer(progressReader);
        }

        private void Registrer(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter); 
                
            ProgressReaders.Add(progressReader);
        }
    }
}