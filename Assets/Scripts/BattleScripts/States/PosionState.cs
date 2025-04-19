using UnityEngine;

public class PoisonState : IUnitState
{

    private Unit unit;

    private int timer = 3;
    private int damagePerTurn = 5;

    public PoisonState(Unit character)
    {
        unit = character;
    }


    public void Enter() 
    {
    }

    public void Execute()
    {
        if (timer > 0)
        {
            unit.DamageUnit(damagePerTurn);
            timer--;
        }
        else
        {
            unit.ChangeState(new DefaultState(unit));
        }
    }

    public void Exit() 
    { 
    }
}
