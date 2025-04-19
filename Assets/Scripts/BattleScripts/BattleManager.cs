using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;


public class BattleManager : MonoBehaviour
{
    private Hero hero;
    private Enemy enemy;
    private Unit currentTurnTaker;
    public TMP_Text targetNameAndHPText;
    public TMP_Text heroNameAndHPText;
    public TMP_Text currentTurnTakerText;

    void Start()
    {
        SpawnHero();
        SpawnEnemy();
        SetStartingUnit();
        TakeAction();
    }

    private void TakeAction()
    {
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
        GameObject.Find("AttackButton").GetComponent<Button>().interactable = false;

    }

    private void EnablePlayerButtons()
    {
        GameObject.Find("AttackButton").GetComponent<Button>().interactable = true;

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
