using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory  _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IUncollectedLootChecker _uncollectedLootChecker;
        private readonly IStaticDataService _staticData;
        
        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService, 
            IUncollectedLootChecker uncollectedLootChecker, IStaticDataService staticData)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _uncollectedLootChecker = uncollectedLootChecker;
            _staticData = staticData;
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
            _uncollectedLootChecker.Init(_gameFactory);
            InitSpawners();
            
            var initialPoint = Object.FindObjectOfType<InitialPoint>();
            var hero = _gameFactory.CreateHero(initialPoint);
            
            InitHud(hero);
            InitUncollectedLoot();
            
            CameraFollow(hero.transform);
        }

        private void InitSpawners()
        {
            var sceneKey = SceneManager.GetActiveScene().name;
            var levelData = _staticData.ForLevel(sceneKey);

            foreach (var spawner in levelData.EnemySpawners)
            {
                _gameFactory.CreateSpawner(spawner.Position, spawner.Id, spawner.MonsterTypeId);
            }
        }

        private void InitUncollectedLoot()
        {
            _gameFactory.Register(_uncollectedLootChecker);
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