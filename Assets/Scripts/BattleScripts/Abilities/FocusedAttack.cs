using UnityEngine;

[CreateAssetMenu(fileName = "FocusedAttack", menuName = "Abilities/FocusedAttack")]

public class FocusedAttack : AbilityData
{

    public int timer;
    public override void UseAbility(Unit unit)
    {
        unit.ChangeState(new FocusedState(unit, unit, timer, power));

    }

    public void Focused(Unit unit, Unit target)
    {
        unit.ChangeState(new FocusedState(unit, target, timer, power));

    }
}
