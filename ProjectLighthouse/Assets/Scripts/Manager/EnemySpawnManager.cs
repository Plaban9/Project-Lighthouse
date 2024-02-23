using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> _enemyPrefabs = new List<Enemy>();

    [SerializeField]
    private List<Transform> _respawnPoints = new List<Transform>();

    private List<GameObject> _currentEnemyList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
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
        int spawnIndex = Random.Range(0, _respawnPoints.Count - 1);
        Enemy spawn = Instantiate(_enemyPrefabs[0]);
        _currentEnemyList.Add(spawn.gameObject);

        spawn.transform.position = _respawnPoints[spawnIndex].position;

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
