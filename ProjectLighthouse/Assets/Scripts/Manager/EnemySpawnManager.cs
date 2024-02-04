using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> _enemyPrefabs = new List<Enemy>();

    [SerializeField]
    private List<Transform> _respawnPoints = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
