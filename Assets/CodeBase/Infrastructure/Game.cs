using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Logic;

namespace CodeBase.Infrastructure
{
    public class Game 
    {
        public GameStateMachine StateMachine;
        
        private SceneLoader _sceneLoader;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
        {
            _sceneLoader = new SceneLoader(coroutineRunner);
            StateMachine = new GameStateMachine(_sceneLoader, loadingCurtain, AllServices.Container);
        }
    }
}
