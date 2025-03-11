using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services.Input;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly AllServices _allServices;
        private SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _allServices = services;
            
            RegisterServices();
        }
        
        public void Enter()
        {
            _sceneLoader.Load(SceneNames.InitScene, EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {
        }

        private void RegisterServices()
        {
            RegisterStaticData();
            _allServices.RegisterSingle<IInputService>(InputService());
            _allServices.RegisterSingle<IAssetProvider>(new AssetProvider());
            _allServices.RegisterSingle<IGameFactory>(
                new GameFactory(_allServices.Single<IAssetProvider>(), _allServices.Single<IStaticDataService>()));
            _allServices.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _allServices.RegisterSingle<ISaveLoadService>(new SaveLoadService(_allServices.Single<IPersistentProgressService>(), 
                _allServices.Single<IGameFactory>()));
            
        }

        private void RegisterStaticData()
        {
            var staticDataService = new StaticDataService();
            staticDataService.LoadMonsters();
            _allServices.RegisterSingle<IStaticDataService>(staticDataService);
        }

        private IInputService InputService()
        {
            if (Application.isEditor)
                return new StandaloneInputService();
            else
                return new MobileInputService();
        }
    }
}