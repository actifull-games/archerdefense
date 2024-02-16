using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossSpawner : MonoBehaviour
{
    public static BossSpawner instance;
    public float startSpawnRadius;
    
    public GameObject _bossPrefab, bossAnimUI;
    
    public List<EnemySpawner> spawners = new List<EnemySpawner>();
    public float timeToSpawnBoss,maxTimeSpawnAll;
    private float timer,timerBoss;
    public bool isBossDesd;
    private bool bossSpawned;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameManager.instance.AddEnemyCount(1);
    }
    
    
    void Update()
    {
        if (GameManager.instance.isStartGame && !GameManager.instance.isGameFailed)
        {
            if (!isBossDesd)
            {
                if (bossSpawned)
                {
                    if (timer < maxTimeSpawnAll)
                    {
                        timer += 1f * Time.deltaTime;
                    }
                    else
                    {
                        timer = 0f;
                        foreach (var spawner in spawners)
                        {
                            spawner.SpawnAtBoss();
                        }
                    }
                }
                else
                {
                    if (timerBoss < timeToSpawnBoss)
                    {
                        timerBoss += 1f * Time.deltaTime;
                        if (timeToSpawnBoss - 5f < timerBoss)
                        {
                            bossAnimUI.SetActive(true);
                        }
                    }
                    else
                    {
                        bossAnimUI.SetActive(false);
                        timerBoss = 0f;
                        bossSpawned = true;
                        GameObject enemyGO = Instantiate(_bossPrefab, transform.position,
                            Quaternion.identity);
                        enemyGO.GetComponent<Enemy>()._Base = TowerController.instance.towerVisual;
                    }
                }
            }
        }
    }
    
    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }
}
