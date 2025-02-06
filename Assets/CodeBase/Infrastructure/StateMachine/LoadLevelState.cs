using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        private void OnLoaded()
        {
            var initialPoint = Object.FindObjectOfType<InitialPoint>();
            
            var hero = InstantiatePrefab("Hero/hero", initialPoint.transform.position);
            InstantiatePrefab("Hud/Hud");
            
            CameraFollow(hero.transform);
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
            throw new System.NotImplementedException();
        }
    }
}