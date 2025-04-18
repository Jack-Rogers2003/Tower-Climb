using UnityEngine;

public class PoisonState : IUnitState
{

    private Unit unit;

    private int timer = 3;
    private int damagePerTurn = 10;

    public PoisonState(Unit character)
    {
        this.unit = character;
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

        }
    }

    public void Exit() { Debug.Log("Exiting Idle State"); }
}
