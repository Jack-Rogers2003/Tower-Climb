using UnityEngine;
using System.IO;
using UnityEditor;

public abstract class Unit : MonoBehaviour
{
    // Common properties for all units
    public string unitName;
    private int health;
    private int attackPower;
    private int defence;
    private int speed;
    private Sprite unitSprite;
    private bool isDead = false;
    private string filePath = Path.Combine(Application.dataPath, "BattleAssets");


    public bool IsAlive()
    {
        return isDead;
    }

    public void Attack(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }

    public void Initialize(string filePathToUnit)
    {
        string pathForStats = filePath + filePathToUnit + "/stats.txt";
        if (File.Exists(pathForStats))
        {
            StreamReader reader = new StreamReader(pathForStats);
            unitName = reader.ReadLine();
            health = int.Parse(reader.ReadLine());
            attackPower = int.Parse(reader.ReadLine());
            defence = int.Parse(reader.ReadLine());
            speed = int.Parse(reader.ReadLine());
            reader.Close();
        }
        byte[] fileData = File.ReadAllBytes("Assets/BattleAssets" + filePathToUnit + "/sprite.png");

        // Create a new Texture2D object
        Texture2D texture = new Texture2D(1, 1); 
        texture.LoadImage(fileData); 

        unitSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public int GetAttackPower()
    {
        return attackPower;
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
