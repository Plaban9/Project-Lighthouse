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
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SpawnRock()
    {
        bool canSpawnTurrent = Random.Range(0, 2) == 0;

        var selectedPrefab = canSpawnTurrent ? _turretsPrefabs[Random.Range(0, _turretsPrefabs.Length - 1)] : _landsPrefabs[Random.Range(0, _landsPrefabs.Length - 1)];

        var go = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
        go.transform.SetParent(transform);
        go.transform.localScale = Vector3.one;
    }
}
