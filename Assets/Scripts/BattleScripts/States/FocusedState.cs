using UnityEngine;

public class FocusedState : IUnitState
{

    private readonly Unit unit;
    private readonly Unit target;

    private int timer;
    private readonly int damageAfterFocus;


    public FocusedState(Unit character, Unit target, int timer, int damage)
    {
        unit = character;
        this.target = target;
        this.timer = timer;
        damageAfterFocus = damage;
    }


    public void Enter()
    {
    }

    public void Execute()
    {
        Debug.Log(timer);
        if (timer == 0)
        {
            target.DamageUnit(damageAfterFocus);
            unit.ChangeState(new DefaultState(unit));
        }
        else
        {
            timer--;
        }
    }

    public void Exit()
    {
    }
}
