using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesSpawngroup
{
    public EnemyData enemyData;
    public int count;
    public bool isBoss;

    public float repeatTimer;
    public float timeBetweenSpawn;
    public int repeatCount;

    public EnemiesSpawngroup(EnemyData enemyData, int count, bool isBoss)
    {
        this.enemyData = enemyData;
        this.count = count;
        this.isBoss = isBoss;
    }

    public void SetRepeatSpawn( float timeBetweenSpawns, int repeatCount)
    {
        this.timeBetweenSpawn = timeBetweenSpawns;
        this.repeatCount = repeatCount;
        repeatTimer = timeBetweenSpawn;
    }
}

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] StageProgress stageProgress;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject enemyAnimation;
    [SerializeField] Vector2 spawnArea;
    [SerializeField] float spawnTimer;
    private GameObject player;
    [SerializeField] Slider bossHealthBar;

    List<Enemy> bossEnemiesList;
    int totalBossHealth;
    int currentBossHealth;

    List<EnemiesSpawngroup> enemiesSpawngroupsList;
    List<EnemiesSpawngroup> repeatedSpawnGroupList;


    private void Start()
    {
        player = GameManager.instance.playerTransform.gameObject;
        bossHealthBar = FindObjectOfType<BossHPBar>(true).GetComponent<Slider>();
    }

    private void Update()
    {
        ProcessSpawn();
        ProcessRepeatedSpawnGroups();
        UpdateBossHealth();
    }

    private void ProcessRepeatedSpawnGroups()
    {
        if (repeatedSpawnGroupList == null)
        {
            return;
        }
        for(int i = repeatedSpawnGroupList.Count -1; i>=0; i--)
        {
            repeatedSpawnGroupList[i].repeatTimer -= Time.deltaTime;
            if(repeatedSpawnGroupList[i].repeatTimer < 0)
            {
                repeatedSpawnGroupList[i].repeatTimer = repeatedSpawnGroupList[i].timeBetweenSpawn;
                AddGroupeToSpawn(repeatedSpawnGroupList[i].enemyData, repeatedSpawnGroupList[i].count, repeatedSpawnGroupList[i].isBoss);
                repeatedSpawnGroupList[i].repeatCount -= 1;
                if (repeatedSpawnGroupList[i].repeatCount <= 0)
                {
                    repeatedSpawnGroupList.RemoveAt(i);
                }
            }
        }
    }

    private void ProcessSpawn()
    {
        if(enemiesSpawngroupsList == null) { return; }

        if (enemiesSpawngroupsList.Count > 0)
        {
            SpawnEnemy(enemiesSpawngroupsList[0].enemyData, enemiesSpawngroupsList[0].isBoss);
            enemiesSpawngroupsList[0].count -= 1;

            if (enemiesSpawngroupsList[0].count <= 0)
            {
                enemiesSpawngroupsList.RemoveAt(0);
            }
        }
        
    }

    private void UpdateBossHealth()
    {
        if(bossEnemiesList == null) { return; }
        if(bossEnemiesList.Count == 0) { return; }
        currentBossHealth = 0;
        for(int i = 0; i< bossEnemiesList.Count; i++)
        {
            if(bossEnemiesList[i] == null) { continue; }
            currentBossHealth += bossEnemiesList[i].stats.hp;
        }

        bossHealthBar.value = currentBossHealth;

        if (currentBossHealth <= 0)
        {
            bossHealthBar.gameObject.SetActive(false);
            bossEnemiesList.Clear();
        }
    }

    public void AddRepeatedSpawn(StageEvent stageEvent, bool isBoss)
    {
        EnemiesSpawngroup repeatSpawnGroup = new EnemiesSpawngroup(stageEvent.enemyToSpawn, stageEvent.count, isBoss);
        repeatSpawnGroup.SetRepeatSpawn(stageEvent.repeatEverySeconds, stageEvent.repeatCount);

        if(repeatedSpawnGroupList == null)
        {
            repeatedSpawnGroupList = new List<EnemiesSpawngroup>();
        }

        repeatedSpawnGroupList.Add(repeatSpawnGroup);
    }

    public void AddGroupeToSpawn(EnemyData enemyToSpawn, int count, bool isBoss)
    {
        EnemiesSpawngroup newGroup = new EnemiesSpawngroup(enemyToSpawn, count, isBoss);
        if(enemiesSpawngroupsList == null) { enemiesSpawngroupsList = new List<EnemiesSpawngroup>(); }

        enemiesSpawngroupsList.Add(newGroup);


    }

    public void SpawnEnemy(EnemyData enemyToSpawn, bool isBoss)
    {
        Vector3 position = UtilityTools.GenerateRandomPositionSquarePattern(spawnArea);

        position += player.transform.position;

        //spawning mainn enemy object
        GameObject newEnemy = Instantiate(enemy);
        newEnemy.transform.position = position;

        Enemy newEnemyComponennt = newEnemy.GetComponent<Enemy>();
        newEnemyComponennt.SetTarget(player);
        newEnemyComponennt.SetStats(enemyToSpawn.stats);
        newEnemyComponennt.UpdateStatsForProgress(stageProgress.Progress);

        if(isBoss == true)
        {
            SpawnBossEnemy(newEnemyComponennt);
        }

        newEnemy.transform.parent = transform;

        //spawning sprite object of the enemy
        GameObject spriteObject = Instantiate(enemyToSpawn.animatedPrefabs);
        spriteObject.transform.parent = newEnemy.transform;
        spriteObject.transform.localPosition = Vector3.zero;
    }

    private void SpawnBossEnemy(Enemy newBoss)
    {
        if(bossEnemiesList == null) { bossEnemiesList = new List<Enemy>(); }

        bossEnemiesList.Add(newBoss);

        totalBossHealth += newBoss.stats.hp;

        bossHealthBar.gameObject.SetActive(true); 
        bossHealthBar.maxValue = totalBossHealth;
    }
}
