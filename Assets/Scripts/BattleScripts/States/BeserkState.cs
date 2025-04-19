using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class BeserkState : IUnitState
{
    private int turns = 3;
    private Unit target;
    private Unit beserker;


    public BeserkState(Unit unit, Unit currentTarget)
    {
        beserker = unit;
        target = currentTarget;
        target.DescreaseDefencebyPercentage(0.75);
        Execute();
    }

    public void Enter()
    {
    }

    public void Execute()
    {
        Debug.Log(turns);
        if (turns != 0)
        {
            target.DamageUnit(15);
            turns--;
        }
        else
        {
            Debug.Log("Here" + turns);
            target.RevertDefence(0.75);
            beserker.ChangeState(new DefaultState(beserker));
        }
    }

    public void Exit()
    {
    }
}
