using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "HealAbility", menuName = "Abilities/HealAbility")]

public class HealAbility : AbilityData
{
    public Vector3 targetPosition;
    public float moveDuration = 1f;
    private Texture2D healIcon;

    public override void UseAbility(Unit target)
    {
        healIcon = Resources.Load<Texture2D>("BattleAssets/heal_icon");
        AudioManager.GetInstance().PlayHealSound();
        target.DamageUnit(-power);
    }
}
