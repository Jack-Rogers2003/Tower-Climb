using UnityEditor;
using UnityEngine;

public enum AbilityType { Attack, Heal, Buff, Debuff, Utility }

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/Ability")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    public string description;

    public AbilityType type;
    public int power; 
    public int maxUsage;

    public virtual void UseAbility(Unit target)
    {
    }

}
