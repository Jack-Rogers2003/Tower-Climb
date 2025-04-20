using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class BattleManager : MonoBehaviour
{
    private Hero hero;
    private Enemy enemy;
    private GameObject attackButton;
    private GameObject abilitiesButton;
    private Unit currentTurnTaker;
    public TMP_Text targetNameAndHPText;
    public TMP_Text heroNameAndHPText;
    public TMP_Text currentTurnTakerText;
    public GameObject abilityPanel;
    List<AbilityData> abilities;


    void Start()
    {
        attackButton = GameObject.Find("AttackButton");
        abilitiesButton = GameObject.Find("AbilitiesButton");


        SpawnHero();
        SpawnEnemy();
        SetStartingUnit();
        SetAbilities();
        HideAbilites();
        TakeAction();
    }

    private void TakeAction()
    {
        Debug.Log(enemy.GetCurrentState());
        AfterAction();
        if (currentTurnTaker == enemy)
        {
            DisablePlayerButtons();
            currentTurnTakerText.text = "Current Turn: " + enemy.GetName();
            StartCoroutine(Wait());
        }
        else
        {
            currentTurnTakerText.text = "Current Turn: " + hero.GetName();
            hero.ExecuteState();
            Debug.Log(hero.GetCurrentState());
            if (hero.GetCurrentState().GetType() == typeof(BeserkState) || hero.GetCurrentState().GetType() == typeof(FocusedState))
            {
                Debug.Log("I am here");
                DisablePlayerButtons();
                currentTurnTaker = enemy;
                TakeAction();
            }
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        EnemyAttack();
        EnablePlayerButtons();

    }

    private void DisablePlayerButtons()
    {
        attackButton.GetComponent<Button>().interactable = false;
        abilitiesButton.GetComponent<Button>().interactable = false;
    }

    private void EnablePlayerButtons()
    {
        if (hero.GetCurrentState().GetType() != typeof(BeserkState) && hero.GetCurrentState().GetType() != typeof(FocusedState))
        {
            attackButton.GetComponent<Button>().interactable = true;
            abilitiesButton.GetComponent<Button>().interactable = true;
        }
    }

    private void AttackButton()
    {
        enemy.DamageUnit(10);
        currentTurnTaker = enemy;
        TakeAction();
    }

    private void EnemyAttack()
    {           
        hero.DamageUnit(10);
        currentTurnTaker = hero;
        enemy.ExecuteState();
        TakeAction();
    }

    public void HideAbilites()
    {
        attackButton.SetActive(true);
        abilitiesButton.SetActive(true);
        abilityPanel.SetActive(false);
    }

    public void ShowAbilities()
    {
        attackButton.SetActive(false);
        abilitiesButton.SetActive(false);
        abilityPanel.SetActive(true);
    }

    private void SetAbilities()
    {
        abilities = hero.GetAbilites();
        int currentAbility = 1;
        foreach(AbilityData ability in abilities)
        {
            abilityPanel.transform.Find("Ability" + currentAbility).GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text = ability.abilityName;
            Debug.Log(ability.description);
            abilityPanel.transform.Find("Ability" + currentAbility).GetComponent<AbilityButtonHover>().SetHoverText(ability.description);

            currentAbility++;
        }
    }

    public void AbilityButtonPress()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        int lastChar = buttonName[^1] - '1';
        AbilityData selectedAbility = abilities[lastChar];
        if (selectedAbility.type == AbilityType.self)
        {
            if (selectedAbility is BeserkAbility beserkAbility)
            {
                beserkAbility.Beserk(hero, enemy);  
            }
            else if (selectedAbility is FocusedAttack focusedAttack)
            {
                focusedAttack.Focused(hero, enemy);

            }
            else
            {
                selectedAbility.UseAbility(hero);
            }
        }
        else
        {
            selectedAbility.UseAbility(enemy);
        }
        HideAbilites();
        currentTurnTaker = enemy;
        TakeAction();
    }

    void AfterAction()
    {
        heroNameAndHPText.text = hero.GetName() + "\nHP: " + hero.GetCurrentHP() + "/" + hero.GetMaxHP();

        if (!enemy.IsAlive())
        {
            PlayerPrefs.SetInt("HasWon", 1);
            int currentBattles = int.Parse(PlayerPrefs.GetString("BattleCount", "0"));
            PlayerPrefs.SetString("BattleCount", (currentBattles + 1).ToString());

            SceneManager.LoadScene("WinLossScreen", LoadSceneMode.Single);
        }
        else if (!hero.IsAlive())
        {
            PlayerPrefs.SetInt("HasWon", 0);
            SceneManager.LoadScene("WinLossScreen", LoadSceneMode.Single);
        }
    }

    private void Update()
    {
        Collider2D spriteCollider = GameObject.Find("EnemyUnit").GetComponent<BoxCollider2D>();
        Vector2 mousePos = new(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // Check if the mouse is over the collider
        if (spriteCollider.OverlapPoint(worldMousePos))
        {
            targetNameAndHPText.text = "Target: " + enemy.GetName() + "  HP: " + enemy.GetCurrentHP() + "/" + enemy.GetMaxHP();
        }
        else
        {
            targetNameAndHPText.text = "";
        }
    }

    private void SetStartingUnit()
    {
        if (enemy.GetSpeed() > hero.GetSpeed())
        {
            currentTurnTaker = enemy;
        }
        else
        {
            currentTurnTaker = hero;
        }
    }


    private void SpawnHero()
    {
        GameObject heroObj = GameObject.Find("HeroUnit");
        hero = heroObj.AddComponent<Hero>();
        hero.Initialize("Hero");
    }

    private void SpawnEnemy()
    {
        GameObject enemyObject = GameObject.Find("EnemyUnit");
        enemy = enemyObject.AddComponent<Enemy>();
        enemy.Initialize("Enemy");
    }

    public void PauseGame()
    {
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Single);
    }
}
