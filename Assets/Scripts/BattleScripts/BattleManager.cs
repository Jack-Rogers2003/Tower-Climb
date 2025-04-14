using UnityEngine;
using System.Collections.Generic;


public class BattleManager : MonoBehaviour
{
    private List<Hero> heroes = new List<Hero>();
    private List<Enemy> enemies = new List<Enemy>();
    private int nextHero = 1;
    private int nextEnemy = 1;

    void Start()
    {
        SpawnHero("/PlayerCharacters/Warrior");
        SpawnHero("/PlayerCharacters/Ninja");
        SpawnHero("/PlayerCharacters/BlackMage");
        SpawnHero("/PlayerCharacters/WhiteMage");
        SpawnEnemy("/EnemyCharacters/Dragon");
        
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
        hero.GetAbilities(filePath);
        nextHero++;
    }

    private void SpawnEnemy(string filePath)
    {
        GameObject enemyObject = GameObject.Find("EnemyUnit" + nextEnemy);
        Enemy enemy = enemyObject.AddComponent<Enemy>();
        enemy.Initialize(filePath);
        enemies.Add(enemy);
        SpriteRenderer spriteRenderer = enemyObject.GetComponent<SpriteRenderer>();
        Sprite toAdd = enemy.GetSprite();
        spriteRenderer.sprite = enemy.GetSprite();
        nextEnemy++;
    }
}
