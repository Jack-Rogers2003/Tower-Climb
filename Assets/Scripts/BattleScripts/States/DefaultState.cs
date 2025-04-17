using UnityEngine.TextCore.Text;

public class DefaultState : IUnitState
{

    private Unit unit;

    public DefaultState(Unit unit)
    {
        this.unit = unit;
    }

    public void Enter()
    {
    }

    public void Execute()
    {
    }

    public void Exit()
    {
    }
}
