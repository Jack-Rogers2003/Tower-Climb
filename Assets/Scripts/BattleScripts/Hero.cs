using System.Collections.Generic;
using UnityEngine;

public class Hero: Unit
{
    private List<AbilityData> allAbilities;
        
    void Awake()
    {
        allAbilities = ChooseAbilityManager.GetChosenAbilites();
    }

    public List<AbilityData> GetAbilites()
    {
        return allAbilities;
    }

}
