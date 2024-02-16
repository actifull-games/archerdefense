using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    //public static EnemySpawner instance;

    public Transform _spawnPoint;

    public List<GameObject> _enemyPrefabs = new List<GameObject>();

    public Transform _Base;

    public float waitTimePerWave;

    private float timer;
    [HideInInspector]
    public bool isStartTime;
    private bool isShowedArrow;
    [System.Serializable]
    public class EnemysCount
    {
        public int smallCount;
        public int standartCount;
        public int bigCount;
    }

    public EnemysCount waveEnemyCount;
    
    public bool isTutorialSpawn;
    
    private void Awake()
    {
       // instance = this;
    }

    void Start()
    {

        if (Tutorial.instance != null)
        {
            if (Tutorial.instance.isTutorialActive)
            {
                isTutorialSpawn = true;
            }
            else
            {
                isTutorialSpawn = false;
            }
        }else
        {
            isTutorialSpawn = false;
        }
        
        if (!isTutorialSpawn)
        {
            isStartTime = true;
        }
        else
        {
            isStartTime = false;
        }

        int count = waveEnemyCount.smallCount + waveEnemyCount.standartCount + waveEnemyCount.bigCount;
        GameManager.instance.AddEnemyCount(count);
    }

    public void SpawnAtBoss()
    {
        if (!isStartTime)
        {
            isStartTime = true;
            int count = waveEnemyCount.smallCount + waveEnemyCount.standartCount + waveEnemyCount.bigCount;
            GameManager.instance.AddEnemyCount(count);
        }
    }
  
    void Update()
    {
        if (GameManager.instance.isStartGame && !GameManager.instance.isGameFailed)
        {
            if (isStartTime)
            {
                if (timer < waitTimePerWave)
                {
                    timer += 1f * Time.deltaTime;
                    if (waitTimePerWave - 5f < timer)
                    {
                        if (!isShowedArrow)
                        {
                            _spawnPoint.gameObject.SetActive(true);
                            isShowedArrow = true;
                        }
                    }
                }
                else
                {
                    timer = 0;
                    isStartTime = false;
                    SpawnEnemy();
                }
            }
        }
    }
    

    void SpawnEnemy()
    {

        for (int i = 0; i < waveEnemyCount.smallCount; i++)
        {
            Vector3 randPos = new Vector3(
                Random.Range(_spawnPoint.position.x - 3f,
                    _spawnPoint.position.x + 3f),
                _spawnPoint.position.y,
                Random.Range(_spawnPoint.position.z - 3f,
                    _spawnPoint.position.z + 3f));
            GameObject enemyGO = Instantiate(_enemyPrefabs[0], randPos,
                Quaternion.identity);
            enemyGO.GetComponent<Enemy>()._Base = _Base;
        }

        for (int i = 0; i < waveEnemyCount.standartCount; i++)
        {
            Vector3 randPos = new Vector3(
                Random.Range(_spawnPoint.position.x - 3f,
                    _spawnPoint.position.x + 3f),
                _spawnPoint.position.y,
                Random.Range(_spawnPoint.position.z - 3f,
                    _spawnPoint.position.z + 3f));
            GameObject enemyGO = Instantiate(_enemyPrefabs[1], randPos,
                Quaternion.identity);
            enemyGO.GetComponent<Enemy>()._Base = _Base;
        }

        for (int i = 0; i < waveEnemyCount.bigCount; i++)
        {
            Vector3 randPos = new Vector3(
                Random.Range(_spawnPoint.position.x - 3f,
                    _spawnPoint.position.x + 3f),
                _spawnPoint.position.y,
                Random.Range(_spawnPoint.position.z - 3f,
                    _spawnPoint.position.z + 3f));
            GameObject enemyGO = Instantiate(_enemyPrefabs[2], randPos,
                Quaternion.identity);
            enemyGO.GetComponent<Enemy>()._Base = _Base;
        }

        _spawnPoint.gameObject.SetActive(false);


    }
}
    

