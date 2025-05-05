using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseAbilityManager : MonoBehaviour
{
    public Transform contentArea; 
    public GameObject togglePrefab; 
    private static readonly List<AbilityData> chosenAbilities = new();
    private readonly List<(Toggle, UnityAction<bool>)> toggles = new();



    void Start()
    {
        AbilityData[] allAbilities = Resources.LoadAll<AbilityData>("Abilities");
        foreach (AbilityData ability in allAbilities)
        {
            AddToggle(ability);
        }
    }

    void AddToggle(AbilityData abilityData)
    {
        GameObject toggleObj = Instantiate(togglePrefab, contentArea);
        Toggle toggle = toggleObj.GetComponent<Toggle>();
        Text label = toggleObj.GetComponentInChildren<Text>(); // assumes only one Text
        label.text = "Name: " + abilityData.abilityName + "\nDescription: " + abilityData.description + "\nPower: " + abilityData.power;
        toggle.isOn = false;
        void toggleListener(bool val) => AddAbility(toggle, abilityData);
        toggle.onValueChanged.AddListener(toggleListener);
        toggles.Add((toggle, toggleListener));
    }

    public void AddAbility(Toggle t, AbilityData ability)
    {
        t = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();
        if (chosenAbilities.Count < 4 && t.isOn)
        {
            chosenAbilities.Add(ability);
        }
        else if (chosenAbilities.Count <= 4 && !t.isOn)
        {
            chosenAbilities.Remove(ability);
        }
        else
        {
            var unique = toggles
                .Where(x => x.Item1 == t)
                .Select(x => x.Item2)
                .Distinct()
                .Single();
            t.onValueChanged.RemoveListener(unique);
            t.isOn = false;
            t.onValueChanged.AddListener(unique);
        }
    }

    public void StartBattleButton()
    {
        SceneManager.LoadScene("BattleScreen", LoadSceneMode.Single);

    }

    public static List<AbilityData> GetChosenAbilites()
    {
        return chosenAbilities;
    }
}
