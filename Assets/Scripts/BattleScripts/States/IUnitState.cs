public interface IUnitState
{
    void Enter();
    void Execute();
    void Exit();


    string ToString();
}