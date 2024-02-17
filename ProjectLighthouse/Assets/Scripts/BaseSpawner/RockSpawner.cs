using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _turretsPrefabs;

    [SerializeField]
    private GameObject[] _landsPrefabs;

    public void Start()
    {
        //SpawnRock();
    }

    public void SpawnRock() 
    {
        bool rand = Random.Range(0, 2) % 2 == 0;

        if (rand) 
        {
            GameObject temp = Instantiate(_turretsPrefabs[Random.Range(0, _turretsPrefabs.Length - 1)], transform.position, Quaternion.identity);
            temp.transform.SetParent(this.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
        else 
        {
            //spawn land area
            GameObject temp = Instantiate(_landsPrefabs[Random.Range(0, _landsPrefabs.Length - 1)], transform.position, Quaternion.identity);
            temp.transform.SetParent(this.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
        }


    }

}
