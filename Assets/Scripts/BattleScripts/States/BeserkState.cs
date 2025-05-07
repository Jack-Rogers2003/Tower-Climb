using UnityEngine;

public class BeserkState : IUnitState
{
    private int turns;
    private readonly Unit target;
    private readonly Unit beserker;
    private readonly int damage;


    public BeserkState(Unit unit, Unit currentTarget, int timer, int damage)
    {
        beserker = unit;
        target = currentTarget;
        turns = timer;
        this.damage = damage;
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
            target.DamageUnit(damage);
            turns--;
        }
        else
        {
            beserker.ChangeState(new DefaultState(beserker));
        }
    }

    public void Exit()
    {
    }

    public string ToString()
    {
        return "Beserk";
    }
}
