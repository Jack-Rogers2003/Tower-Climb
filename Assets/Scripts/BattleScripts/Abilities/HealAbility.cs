using UnityEngine;

[CreateAssetMenu(fileName = "HealAbility", menuName = "Abilities/HealAbility")]

public class HealAbility : AbilityData
{
    public override void UseAbility(Unit target)
    {
        target.DamageUnit(-power);
    }
}
