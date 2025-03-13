using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;

namespace CodeBase.Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _state;
        private IExitableState _currentState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
        {
            _state = new Dictionary<Type, IExitableState>()
            {
                {typeof(BootstrapState), new BootstrapState(this, sceneLoader, services)},
                {typeof(LoadLevelState), new LoadLevelState(this, sceneLoader, loadingCurtain, services.Single<IGameFactory>(), 
                    services.Single<IPersistentProgressService>(), services.Single<IUncollectedLootChecker>())},
                {typeof(LoadProgressState), new LoadProgressState(this, 
                    services.Single<IPersistentProgressService>(), services.Single<ISaveLoadService>())},
                {typeof(GameLoopState), new GameLoopState()}
            };
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        { 
            
            var state = ChangeState<TState>(); 
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();
            var state = GetState<TState>();
            _currentState = state;
            return state; 
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _state[typeof(TState)] as TState;
    }
}