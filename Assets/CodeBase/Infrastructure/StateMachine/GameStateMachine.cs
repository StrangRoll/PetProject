using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IState> _state;
        private IState _currentState;

        public GameStateMachine(SceneLoader sceneLoader)
        {
            _state = new Dictionary<Type, IState>()
            {
                {typeof(BootstrapState), new BootstrapState(this, sceneLoader)}
            };
        }
        
        public void Enter<TState>() where TState : IState
        {
            _currentState?.Exit();
            var state = _state[typeof(TState)];
            _currentState = state;
            state.Enter();
        }
    }
}
