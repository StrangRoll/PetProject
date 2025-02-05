using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }
        
        public void Enter()
        {
            RegisterServices();
            _sceneLoader.Load(SceneNames.InitScene, EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState>();
        }

        public void Exit()
        {
        }

        private void RegisterServices()
        {
            Game.InputService = RegisterInputService();
        }

        private IInputService RegisterInputService()
        {
            if (Application.isEditor)
                return new StandaloneInputService();
            else
                return new MobileInputService();
        }
    }
}