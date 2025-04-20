using UnityEngine;

[CreateAssetMenu(fileName = "BeserkAbility", menuName = "Abilities/BeserkAbility")]

public class BeserkAbility : AbilityData
{
    public int timer;
    public override void UseAbility(Unit unit)
    {
        unit.ChangeState(new BeserkState(unit, unit, timer, power));

    }

    public void Beserk(Unit unit, Unit target)
    {
        unit.ChangeState(new BeserkState(unit, target, timer, power));

    }
}
