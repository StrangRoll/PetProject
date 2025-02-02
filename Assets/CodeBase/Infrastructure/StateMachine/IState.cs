namespace CodeBase.Infrastructure.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit();
    }
}