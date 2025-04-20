using UnityEngine;

public class PoisonState : IUnitState
{

    private Unit unit;

    private int timer;
    private int damagePerTurn;

    public PoisonState(Unit character, int timer, int damage)
    {
        unit = character;
        this.timer = timer;
        damagePerTurn = damage;
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
