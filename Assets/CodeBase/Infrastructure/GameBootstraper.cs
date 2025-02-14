using CodeBase.Infrastructure.StateMachine;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstraper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain _loadingCurtainPrefab;
        
        private Game _game;
        
        private void Awake()
        {
            _game = new Game(this, Instantiate(_loadingCurtainPrefab));
            _game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}
