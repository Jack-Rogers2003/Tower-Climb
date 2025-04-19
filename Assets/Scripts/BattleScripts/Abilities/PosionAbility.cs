using UnityEngine;

[CreateAssetMenu(fileName = "PoisonAbility", menuName = "Abilities/PoisonAbility")]
public class PoisonAbility : AbilityData
{

    public override void UseAbility(Unit target)
    {
        target.ChangeState(new PoisonState(target));

    }

    private void OnEnable()
    {
        type = AbilityType.enemy;
        abilityName = "Poison";
        description = "Poison the target for 3 turns, dealing 5 damage every turn";
        power = 0;
    }
}
