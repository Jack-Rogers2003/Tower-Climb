using UnityEngine;

[CreateAssetMenu(fileName = "PoisonAbility", menuName = "Abilities/PoisonAbility")]
public class PoisonAbility : AbilityData
{
    public int timer;
    public override void UseAbility(Unit target)
    {
        AudioManager.GetInstance().PlayPoisonSound();
        target.ChangeState(new PoisonState(target, timer, power));

    }
}
