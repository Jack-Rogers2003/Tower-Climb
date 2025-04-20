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

        if (timer == 0)
        {
            int randomNumber = Random.Range(1, 11);
            if (randomNumber >= 1 && randomNumber <= 5)
            {
                target.DamageUnit(damageAfterFocus);
            }
            else
            {
                Debug.Log("Broken!");
            }
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
