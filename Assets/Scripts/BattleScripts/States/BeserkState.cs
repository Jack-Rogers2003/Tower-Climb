using UnityEngine;

public class BeserkState : IUnitState
{
    private int turns;
    private readonly Unit target;
    private readonly Unit beserker;
    private readonly int damage;
    private readonly Sprite sprite = Resources.Load<Sprite>("Status/beserk_icon");
    private readonly GameObject statusObject = GameObject.Find("HeroStatus");


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
        statusObject.SetActive(true);
        if (statusObject && sprite)
        {
            SpriteRenderer renderer = statusObject.GetComponent<SpriteRenderer>();
            if (renderer)
            {
                renderer.sprite = sprite;
            }
        }
    }

    public void Execute()
    {
        Debug.Log(turns);
        if (turns != 0)
        {
            AudioManager.GetInstance().PlaySwordAttack();
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
        SpriteRenderer renderer = statusObject.GetComponent<SpriteRenderer>();

        if (renderer)
        {
            renderer.sprite = null;
        }
    }

    override
    public string ToString()
    {
        return "Beserk";
    }
}
