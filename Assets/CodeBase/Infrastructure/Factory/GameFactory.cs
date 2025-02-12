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

        public GameFactory(IAssetProvider assets)
        {
            _assets = assets;
        }
        public GameObject CreateHero(InitialPoint at) => 
            InstantiateRegistred(AssetPath.HeroPath, at.transform.position);

        private GameObject InstantiateRegistred(string prefab, Vector3 at = default)
        {
            var gameObject = _assets.InstantiatePrefab(prefab, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        public void CreateHud() => 
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