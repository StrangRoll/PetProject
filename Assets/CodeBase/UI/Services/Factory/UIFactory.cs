using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UIRoot";
        
        private IAssetProvider _assets;
        private IStaticDataService _staticData;
        private Transform _uiRoot;
        private IPersistentProgressService progressService;

        public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _assets = assets;
            _staticData = staticData;
            this.progressService = progressService;
        }
        
        public void CreateShop()
        {
            var config = _staticData.ForWindow(WindowId.Shop);
            var window = Object.Instantiate(config.Prefab, _uiRoot);
            window.Construct(progressService);
        }

        public void CreateUIRoot() => 
            _uiRoot = _assets.InstantiatePrefab(UIRootPath).transform;
    }
}