using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstraper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;
        
        private void Awake()
        {
            _game = new Game(this);
            _game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}
