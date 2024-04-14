using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;

    [SerializeField]
    private List<Enemy> _enemyPrefabs = new List<Enemy>();

    [SerializeField]
    private List<Transform> _respawnPoints = new List<Transform>();

    [SerializeField]
    private List<GameObject> _currentEnemyList = new List<GameObject>();

    private int _enemiesToSpawn = -1;

    private bool night = false;

    private void OnEnable()
    {
        TimeController.dayNightCycleStartNotifier += DayNightCycle;
    }

    private void OnDisable()
    {
        TimeController.dayNightCycleStartNotifier -= DayNightCycle;
    }

    private void DayNightCycle(DayNightCycle cycle)
    {
        switch (cycle)
        {
            case global::DayNightCycle.DAY:
                night = false;
                Debug.Log("Enemy Spawn Over");
                CancelInvoke(nameof(SpawnEnemy));
                break;
            case global::DayNightCycle.NIGHT:
                night = true;
                Debug.Log("Enemy Spawn Started");
                InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
                break;
            default:
                Debug.LogError("DayNight Cycle Not Implemented: " + cycle);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    SpawnEnemy();
        //}

        //This is bad, but it will have to do for now


    }

    void SpawnEnemy() 
    {
        if( _enemiesToSpawn > 0)
        {
            _enemiesToSpawn--;

            var spawnPoint = _respawnPoints[Random.Range(0, _respawnPoints.Count - 1)];
            Enemy selectedEnemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count - 1)];
            Enemy spawn = Instantiate(selectedEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
            spawn.SetManager(this);
            _currentEnemyList.Add(spawn.gameObject);
        }
    }

    public void Reset()
    {
        foreach (var item in _currentEnemyList)
        {
            Destroy(item);
        }

        _currentEnemyList.Clear();
    }

    public void SetNumberOfEnemiesToSpawn(int enemiesToSpawn)
    {
        _enemiesToSpawn = enemiesToSpawn;
    }

    public void RemoveEnemy(GameObject gameObject)
    {
        _currentEnemyList.Remove(gameObject);
        if (night && _enemiesToSpawn == 0 && _currentEnemyList.Count == 0)
        {
            GameManager.Instance.NightEnd();
        }
    }
}
