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
    private bool isAlive = true;
    private IUnitState currentState;


    private void Awake()
    {
        ChangeState(new DefaultState(this));

    }

    public void ExecuteState()
    {
        currentState.Execute();
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

    public void Initialize(string unit)
    {
        Debug.Log(unit);
        TextAsset textAsset = Resources.Load<TextAsset>(unit + "/Stats");

        StreamReader reader = new(new MemoryStream(textAsset.bytes));
        unitName = reader.ReadLine();
        maxHealth = int.Parse(reader.ReadLine());
        currentHealth = maxHealth;
        attackPower = int.Parse(reader.ReadLine());
        defence = int.Parse(reader.ReadLine());
        speed = int.Parse(reader.ReadLine());
        currentState = new DefaultState(this);
        reader.Close();

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

    public IUnitState GetCurrentState()
    {
        return currentState;
    }

    public void DescreaseDefencebyPercentage(double percentage)
    {
        defence = (int)(defence * percentage);
    }

    public void RevertDefence(double percentage)
    {
        defence = (int)(defence / percentage);
    }
}
