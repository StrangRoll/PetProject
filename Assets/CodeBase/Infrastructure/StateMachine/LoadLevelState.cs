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
        private readonly IUncollectedLootChecker _uncollectedLootChecker;
        
        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService, IUncollectedLootChecker uncollectedLootChecker)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _uncollectedLootChecker = uncollectedLootChecker;
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
            ReSimpleButtonInput.UpdateButtonsUpdateButtons();
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
            InitUsingTag<EnemySpawner>(GameTags.EnemySpawner);
            
            var initialPoint = Object.FindObjectOfType<InitialPoint>();
            var hero = _gameFactory.CreateHero(initialPoint);
            
            InitHud(hero);
            InitUncollectedLoot();
            
            CameraFollow(hero.transform);
        }

        private void InitUncollectedLoot()
        {
            _uncollectedLootChecker.Init(_gameFactory);
            _gameFactory.Register(_uncollectedLootChecker);
        }

        private void InitUsingTag<T>(string tag) where T : ISavedProgressReader
        {
            foreach (var gameObject in GameObject.FindGameObjectsWithTag(tag))
            {
                var element = gameObject.GetComponent<T>();
                _gameFactory.Register(element);
            }
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