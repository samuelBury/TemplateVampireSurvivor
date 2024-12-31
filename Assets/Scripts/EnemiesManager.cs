using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] StageProgress stageProgress;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject enemyAnimation;
    [SerializeField] Vector2 spawnArea;
    [SerializeField] float spawnTimer;
    private GameObject player;
    float timer;

    private void Start()
    {
        player = GameManager.instance.playerTransform.gameObject;
    }
   /* private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0f)
        {
            SpawnEnemy();
            timer = spawnTimer;
        }
    }*/

    public void SpawnEnemy(EnemyData enemyToSpawn)
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

        newEnemy.transform.parent = transform;

        //spawning sprite object of the enemy
        GameObject spriteObject = Instantiate(enemyToSpawn.animatedPrefabs);
        spriteObject.transform.parent = newEnemy.transform;
        spriteObject.transform.localPosition = Vector3.zero;
    }

   
}
