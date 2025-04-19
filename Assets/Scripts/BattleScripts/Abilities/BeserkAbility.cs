using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "BeserkAbility", menuName = "Abilities/BeserkAbility")]

public class BeserkAbility : AbilityData
{

    public override void UseAbility(Unit unit)
    {
        unit.ChangeState(new BeserkState(unit, unit));

    }

    public void Beserk(Unit unit, Unit target)
    {
        unit.ChangeState(new BeserkState(unit, target));

    }


    private void OnEnable()
    {
        type = AbilityType.self;
        abilityName = "Beserk";
        description = "Attack without stopping for 3 turns, damage up 30%, but defence down 25%";
        power = 0;
    }
}
