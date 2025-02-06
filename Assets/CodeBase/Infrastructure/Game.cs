using CodeBase.Infrastructure.StateMachine;
using CodeBase.Logic;
using CodeBase.Services.Input;

namespace CodeBase.Infrastructure
{
    public class Game 
    {
        public static IInputService InputService;
        public GameStateMachine StateMachine;
        
        private SceneLoader _sceneLoader;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
        {
            _sceneLoader = new SceneLoader(coroutineRunner);
            StateMachine = new GameStateMachine(_sceneLoader, loadingCurtain);
        }
    }
}
