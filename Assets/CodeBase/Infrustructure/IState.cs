namespace CodeBase.Infrustructure
{
    public interface IState
    {
        void Enter();
        void Exit();
    }
}