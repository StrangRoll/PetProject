using System;
using System.Collections.Generic;

namespace CodeBase.Infrustructure
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IState> _state;
        private IState _currentState;

        public GameStateMachine()
        {
            _state = new Dictionary<Type, IState>();
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
