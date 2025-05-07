using UnityEngine.TextCore.Text;

public class DefaultState : IUnitState
{
    private readonly Unit unit;

    public DefaultState()
    {
    }

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

    override
    public string ToString()
    {
        return "Default";
    }
}
