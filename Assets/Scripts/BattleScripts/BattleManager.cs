using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class BattleManager : MonoBehaviour
{
    private Hero hero;
    private Enemy enemy;
    private Unit currentTurnTaker;
    public TMP_Text targetNameAndHPText;
    public TMP_Text currentTurnTakerText;

    private void Awake()
    {
        SpawnHero("/PlayerCharacters/Warrior");
        SpawnEnemy("/EnemyCharacters/Dragon");
        NextRound();
        if (currentTurnTaker = enemy)
        {
            currentTurnTakerText.text = "Current Turn: " + enemy.GetName();
        } else
        {
            currentTurnTakerText.text = "Current Turn: " + hero.GetName();
        }
    }

    public void AttackButton()
    {
        enemy.DamageUnit(10);
    }

    void Update()
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

    private void NextRound()
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


    private void SpawnHero(string filePath)
    {
        GameObject heroObj = GameObject.Find("HeroUnit");
        hero = heroObj.AddComponent<Hero>();
        hero.Initialize(filePath);
        SpriteRenderer spriteRenderer = heroObj.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = hero.GetSprite();
        Debug.Log(hero.GetAttackPower());
    }

    private void SpawnEnemy(string filePath)
    {
        GameObject enemyObject = GameObject.Find("EnemyUnit");
        enemy = enemyObject.AddComponent<Enemy>();
        enemy.Initialize(filePath);
        SpriteRenderer spriteRenderer = enemyObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = enemy.GetSprite();
    }

    public void PauseGame()
    {
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Single);
    }
}
