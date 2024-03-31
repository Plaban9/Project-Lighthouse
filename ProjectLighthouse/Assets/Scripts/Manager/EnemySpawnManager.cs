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

    private List<GameObject> _currentEnemyList = new List<GameObject>();

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
                Debug.Log("Enemy Spawn Over");
                CancelInvoke(nameof(SpawnEnemy));
                break;
            case global::DayNightCycle.NIGHT:
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
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy() 
    {
        var spawnPoint = _respawnPoints[Random.Range(0, _respawnPoints.Count - 1)];
        Enemy selectedEnemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count - 1)];
        Enemy spawn = Instantiate(selectedEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        _currentEnemyList.Add(spawn.gameObject);
    }

    public void Reset()
    {
        foreach (var item in _currentEnemyList)
        {
            Destroy(item);
        }

        _currentEnemyList.Clear();
    }
}
