using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
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
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService, 
            IUncollectedLootChecker uncollectedLootChecker, IStaticDataService staticData, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _uncollectedLootChecker = uncollectedLootChecker;
            _staticData = staticData;
            _uiFactory = uiFactory;
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
            InitUIRoot();
            var hud = _gameFactory.CreateHud();
            
            hud.GetComponent<ActorUI>().Init(hero.GetComponent<HeroHealth>());
        }

        private void InitUIRoot() => 
            _uiFactory.CreateUIRoot();

        private void InformProgressReaders()
        {
            foreach (var progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        private void InitGameWorld()
        {
            var sceneKey = SceneManager.GetActiveScene().name;
            var levelData = _staticData.ForLevel(sceneKey);
            
            _uncollectedLootChecker.Init(_gameFactory);
            
            InitSpawners(levelData);
            var hero = InitHero(levelData.InitialHeroPoint);
            
            InitHud(hero);
            InitUncollectedLoot();
            
            CameraFollow(hero.transform);
        }

        private GameObject InitHero(Vector3 initialPoint) => 
             _gameFactory.CreateHero(initialPoint);

        private void InitSpawners(LevelStaticData levelData)
        {
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