using UnityEngine;
using System.IO;
using Unity.VisualScripting;


public abstract class Unit : MonoBehaviour
{
    // Common properties for all units
    public string unitName;
    private int maxHealth;
    private int currentHealth;
    private int attackPower;
    private int defence;
    private int speed;
    private Sprite unitSprite;
    private bool isAlive = true;
    private readonly string filePath = Path.Combine(Application.dataPath, "BattleAssets");

    private IUnitState currentState;


    private void Awake()
    {
        ChangeState(new DefaultState(this));

    }

    public void ChangeState(IUnitState newState)
    {
        currentState?.Exit();  // Exit the current state
        currentState = newState;
        currentState.Enter();   // Enter the new state
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void DamageUnit(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            isAlive = false;
        }
    }

    public void Initialize(string filePathToUnit)
    {
        string pathForStats = filePath + filePathToUnit + "/stats.txt";
        if (File.Exists(pathForStats))
        {
            StreamReader reader = new(pathForStats);
            unitName = reader.ReadLine();
            maxHealth = int.Parse(reader.ReadLine());
            currentHealth = maxHealth;
            attackPower = int.Parse(reader.ReadLine());
            defence = int.Parse(reader.ReadLine());
            speed = int.Parse(reader.ReadLine());
            reader.Close();
        }
        byte[] fileData = File.ReadAllBytes("Assets/BattleAssets" + filePathToUnit + "/sprite.png");

        // Create a new Texture2D object
        Texture2D texture = new(1, 1); 
        texture.LoadImage(fileData); 

        unitSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public int GetMaxHP()
    {
        return maxHealth;
    }

    public int GetCurrentHP()
    {
        return currentHealth;
    }

    public string GetName()
    {
        return unitName;
    }

    public int GetDefence()
    {
        return defence;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public Sprite GetSprite()
    {
        return unitSprite;
    }
}
