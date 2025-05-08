using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.IO;
using System;


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
    List<AbilityData> abilities = new();
    private readonly AudioManager audioManager = AudioManager.GetInstance();

    private static readonly string saveFilePath = "Assets/Resources/Save/SaveFile.txt";



    void Start()
    {
        audioManager.PlayBattleMusic();

        GameObject heroObj = GameObject.Find("HeroUnit");
        hero = heroObj.AddComponent<Hero>();

        GameObject enemyObject = GameObject.Find("EnemyUnit");
        enemy = enemyObject.AddComponent<Enemy>();

        hero.SetName("Hero");
        enemy.SetName("Dragon");

        int toLoad = PlayerPrefs.GetInt("toLoad", 0);

        attackButton = GameObject.Find("AttackButton");
        abilitiesButton = GameObject.Find("AbilitiesButton");

        if (File.Exists(saveFilePath) && toLoad == 1)
        {
            LoadGame();
        }
        else
        {
            SpawnHero();
            SpawnEnemy();
            SetStartingUnit();
        }
        SetAbilities();
        HideAbilites();
        TakeAction();
    }

    private void LoadGame()
    {
        abilities.Clear();
        hero.GetAbilites().Clear();
        List<string> data = SaveGame.Load();
        PlayerPrefs.SetString("BattleCount", data[0]);
        PlayerPrefs.Save();
        if (data[1] == "Hero")
        {
            currentTurnTaker = hero;
        }
        else
        {
            currentTurnTaker = enemy;
        }
        int abilityTracker = 2;
        AbilityData[] allAbilities = Resources.LoadAll<AbilityData>("Abilities");
        while (!int.TryParse(data[abilityTracker], out _))
        {
            AbilityData obj = null;
            foreach (var ability in allAbilities)
            {
                if (ability.abilityName == data[abilityTracker])
                {
                    obj = ability;
                    break;
                }
            }
            Debug.Log(obj.abilityName);
            hero.AddAbility(obj);
            abilityTracker++;
        }
        hero.SetMaxHP(int.Parse(data[abilityTracker]));
        hero.SetCurrentHP(int.Parse(data[abilityTracker + 1]));

        switch (data[abilityTracker + 2])
        {
            case "Default":
                hero.SetCurrentState(new DefaultState(hero));
                break;
            case "Beserk":
                hero.SetCurrentState(new BeserkState(hero, enemy, 3, 5));
                break;
            case "Focused":
                hero.SetCurrentState(new FocusedState(hero, enemy, 2, 40));
                break;
        }
        enemy.SetMaxHP(int.Parse(data[abilityTracker + 3]));
        enemy.SetCurrentHP(int.Parse(data[abilityTracker + 4]));

        switch (data[abilityTracker + 5])
        {
            case "Poison":
                enemy.SetCurrentState(new PoisonState(enemy, 3, 5));
                break;
        }

    }

    private void TakeAction()
    {
        AfterAction();
        if (currentTurnTaker == enemy)
        {
            DisablePlayerButtons();
            currentTurnTakerText.text = "Current Turn: " + enemy.GetName();
            EnemyAttack();
        }
        else
        {
            currentTurnTakerText.text = "Current Turn: " + hero.GetName();
            hero.ExecuteState();
            if (hero.GetCurrentState().GetType() == typeof(BeserkState) || hero.GetCurrentState().GetType() == typeof(FocusedState))
            {
                DisablePlayerButtons();
                currentTurnTaker = enemy;
                TakeAction();
            }
        }
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
        DisablePlayerButtons();
        StartCoroutine(HeroMoveAndContinue(260));
        audioManager.PlaySwordAttack();
        enemy.DamageUnit(10);
    }

    IEnumerator HeroMoveAndContinue(int moveTo)
    {
        yield return StartCoroutine(MoveX(hero, moveTo));
        currentTurnTaker = enemy;
        TakeAction();
    }

    IEnumerator MoveX(Unit obj, float targetX)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;
        Vector2 targetPos = new(targetX, originalPos.y);
        float t = 0f;

        while (Vector2.Distance(rect.anchoredPosition, targetPos) > 0.1f)
        {
            t += Time.deltaTime * 2f;
            rect.anchoredPosition = Vector2.Lerp(originalPos, targetPos, t);
            yield return null;
        }

        rect.anchoredPosition = targetPos;

        yield return new WaitForSeconds(0.2f);

        t = 0f;
        while (Vector2.Distance(rect.anchoredPosition, originalPos) > 0.1f)
        {
            t += Time.deltaTime * 2f;
            rect.anchoredPosition = Vector2.Lerp(targetPos, originalPos, t);
            yield return null;
        }

        rect.anchoredPosition = originalPos;
    }

    IEnumerator EnemyMoveAndContinue(int moveTo)
    {
        yield return StartCoroutine(MoveX(enemy, moveTo));
        EnablePlayerButtons();
        currentTurnTaker = hero;
        TakeAction();
    }


    private void EnemyAttack()
    {
        StartCoroutine(EnemyMoveAndContinue(-440));
        audioManager.PlayDragonRoar();
        hero.DamageUnit(10);
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
            abilityPanel.transform.Find("Ability" + currentAbility).GetComponent<AbilityButtonHover>().SetHoverText(ability.description);
            currentAbility++;
        }

        for (int i = currentAbility; i < 5; i++)
        {
            Transform abilityButton = abilityPanel.transform.Find("Ability" + i);
            abilityButton.gameObject.SetActive(false);
           
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
        PlayerPrefs.Save();
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
        hero.Initialize("Hero");
    }

    private void SpawnEnemy()
    {
        enemy.Initialize("Enemy");
    }

    public void PauseGame()
    {
        SaveGame.Save(hero, enemy, currentTurnTaker);
        SaveGame.Load();
        audioManager.PauseMusic();
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Single);
    }
}
