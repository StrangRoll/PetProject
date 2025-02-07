using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName, OnLoaded);
            _loadingCurtain.Show();
        }

        private void OnLoaded()
        {
            var initialPoint = Object.FindObjectOfType<InitialPoint>();
            
            var hero = _gameFactory.CreateHero(initialPoint);
            _gameFactory.CreateHud();
            
            CameraFollow(hero.transform);  

            _gameStateMachine.Enter<GameLoopState>();
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