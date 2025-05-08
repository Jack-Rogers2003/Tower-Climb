using UnityEngine;

public class PoisonState : IUnitState
{

    private readonly Unit unit;

    private int timer;
    private readonly int damagePerTurn;
    private readonly Sprite poisonSprite = Resources.Load<Sprite>("Status/poison_icon");
    private readonly GameObject statusObject = GameObject.Find("EnemyStatus");


    public PoisonState(Unit character, int timer, int damage)
    {
        unit = character;
        this.timer = timer;
        damagePerTurn = damage;
    }


    public void Enter() 
    {
        statusObject.SetActive(true);
        if (statusObject && poisonSprite)
        {
            SpriteRenderer renderer = statusObject.GetComponent<SpriteRenderer>();
           if (renderer)
            {
                renderer.sprite = poisonSprite;
            }
        }

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
        SpriteRenderer renderer = statusObject.GetComponent<SpriteRenderer>();

        if (renderer)
        {
            renderer.sprite = null;
        }
    }

    override
    public string ToString()
    {
        return "Poison";
    }
}
