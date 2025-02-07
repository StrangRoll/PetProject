using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;

        public GameFactory(IAssetProvider assets)
        {
            _assets = assets;
        }
        public GameObject CreateHero(InitialPoint at) => 
            _assets.InstantiatePrefab(AssetPath.HeroPath, at.transform.position);

        public void CreateHud() => 
            _assets.InstantiatePrefab(AssetPath.HudPath);
    }
}