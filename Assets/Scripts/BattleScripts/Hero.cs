using System.Collections.Generic;

public class Hero: Unit
{
    public List<AbilityData> allAbilities;
        
    void Start()
    {
        allAbilities = ChooseAbilityManager.GetChosenAbilites();
    }

}
