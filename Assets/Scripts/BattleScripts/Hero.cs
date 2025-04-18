using System.Collections.Generic;
using UnityEngine;

public class Hero: Unit
{
    public List<AbilityData> allAbilities;
        
    void Start()
    {
        allAbilities = ChooseAbilityManager.GetChosenAbilites();
        Debug.Log(allAbilities.Count);

    }

}
