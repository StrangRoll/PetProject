using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName, OnLoaded);
            _gameFactory.CleanUp();
            _loadingCurtain.Show();
        }

        private void OnLoaded()
        {
            InitGameWorld();
            InformProgressReaders();

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitHud(GameObject hero)
        {
            var hud = _gameFactory.CreateHud();
            
            hud.GetComponent<ActorUI>().Init(hero.GetComponent<HeroHealth>());
        }

        private void InformProgressReaders()
        {
            foreach (var progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        private void InitGameWorld()
        {
            var initialPoint = Object.FindObjectOfType<InitialPoint>();
            var hero = _gameFactory.CreateHero(initialPoint);
            
            InitHud(hero);
            
            CameraFollow(hero.transform);
        }

        private void CameraFollow(Transform hero)
        {
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }
    }
}