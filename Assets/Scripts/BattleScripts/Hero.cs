using UnityEngine;

public class Hero: Unit
{
    public AbilityData[] allAbilities;

    void Start()
    {
        allAbilities = Resources.LoadAll<AbilityData>("Abilities");
        // Example: Print all abilities
        foreach (AbilityData ability in allAbilities)
        {
            Debug.Log("Ability: " + ability.abilityName);
        }
    }

}
