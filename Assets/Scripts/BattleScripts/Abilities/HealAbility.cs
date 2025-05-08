using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "HealAbility", menuName = "Abilities/HealAbility")]

public class HealAbility : AbilityData
{
    public override void UseAbility(Unit target)
    {
        AudioManager.GetInstance().PlayHealSound();
        target.DamageUnit(-power);
    }
}
