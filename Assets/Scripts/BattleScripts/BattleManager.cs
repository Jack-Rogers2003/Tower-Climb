using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.UI.CanvasScaler;


public class BattleManager : MonoBehaviour
{
    private List<Hero> heroes = new List<Hero>();
    private List<Unit> enemies;
    private GameObject panel;
    private int nextHero = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel = GameObject.Find("Panel");
        SpawnHero("/PlayerCharacters/Warrior");
        SpawnEnemy("Dragon.txt");
    }

    private void SpawnHero(string filePath)
    {
        GameObject heroObj = GameObject.Find("HeroUnit" + nextHero);
        Hero hero = heroObj.AddComponent<Hero>();
        hero.Initialize(filePath);
        heroes.Add(hero);
        SpriteRenderer spriteRenderer = heroObj.GetComponent<SpriteRenderer>();
        Sprite toAdd = hero.GetSprite();
        spriteRenderer.sprite = hero.GetSprite();
        nextHero++;
    }

    private void SpawnEnemy(string filePath)
    {
        /*
        GameObject enemnyObj = new GameObject("Enemy");
        Enemy enemy = enemnyObj.GetComponent<Enemy>();
        enemy.Initialize(filePath);
        enemies.Add(enemy);
        */
    }
}
