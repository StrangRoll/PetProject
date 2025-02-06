using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string HeroPath = "Hero/hero";
        private const string HudPath = "Hud/Hud";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

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
            
            var hero = InstantiatePrefab(HeroPath, initialPoint.transform.position);
            InstantiatePrefab(HudPath);
            
            CameraFollow(hero.transform);  

            _gameStateMachine.Enter<GameLoopState>();
        }
        
        private void CameraFollow(Transform hero)
        {
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);
        }
        
        private static GameObject InstantiatePrefab(string path, Vector3 at = default)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity); 
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }
    }
}