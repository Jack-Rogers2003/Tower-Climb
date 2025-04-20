using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "PoisonAbility", menuName = "Abilities/PoisonAbility")]
public class PoisonAbility : AbilityData
{
    public int timer;
    public override void UseAbility(Unit target)
    {
        target.ChangeState(new PoisonState(target, timer, power));

    }
}
