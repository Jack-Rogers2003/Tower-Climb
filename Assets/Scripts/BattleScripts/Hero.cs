using System.Collections.Generic;
using UnityEngine;

public class Hero: Unit
{
    private List<AbilityData> allAbilities = new();
        
    void Awake()
    {
        allAbilities.Clear();
        allAbilities = ChooseAbilityManager.GetChosenAbilites();
    }

    public List<AbilityData> GetAbilites()
    {
        return allAbilities;
    }

    public void AddAbility(AbilityData newAbility)
    {
        allAbilities.Add(newAbility);
    }

}
