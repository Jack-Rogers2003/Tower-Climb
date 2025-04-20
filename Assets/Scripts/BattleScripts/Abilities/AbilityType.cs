using UnityEngine;

public enum AbilityType { self, enemy }

public class AbilityData : ScriptableObject
{
    public string abilityName;
    public string description;

    public AbilityType type;
    public int power; 
    public virtual void UseAbility(Unit target)
    {

    }

}
