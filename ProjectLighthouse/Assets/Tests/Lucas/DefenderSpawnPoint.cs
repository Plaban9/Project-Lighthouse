using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DefenderSpawnPointStatus
{
    Available,
    Occupied
}

public class DefenderSpawnPoint : MonoBehaviour
{
    MeshRenderer baseMesh;

    [Header("Materials")]
    [SerializeField] Material normalMaterial;
    [SerializeField] Material onSelectMaterial;
    [SerializeField] Material onOccupiedMaterial;

    [Header("Spawning")]
    [SerializeField] Transform spawnPosition;

    DefenderSpawnPointStatus status = DefenderSpawnPointStatus.Available;
    RaycastHit hitData;
    DefenderSpawnManager DSM;

    void Start()
    {
        baseMesh = GetComponent<MeshRenderer>();
        DSM = DefenderSpawnManager.Instance;

        if(spawnPosition == null)
        {
            spawnPosition = transform.Find("SpawnPosition");
        }
    }

    void Update()
    {
        if (!DSM.IsDraggingDefenderFromMenu())
        {
            SetOnSelect(false);
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        SetOnSelect(Physics.Raycast(ray, out hitData, 1000) && hitData.transform == transform);
    }

    void SetOnSelect(bool set)
    {
        if(set)
        {
            if(status == DefenderSpawnPointStatus.Available && baseMesh.material != onSelectMaterial)
            {
                baseMesh.material = onSelectMaterial;
            }
            else if(status == DefenderSpawnPointStatus.Occupied && baseMesh.material != onOccupiedMaterial)
            {
                baseMesh.material = onOccupiedMaterial;
            }
        }
        else
        {
            if(baseMesh.material != normalMaterial)
            {
                baseMesh.material = normalMaterial;
            }
        }
    }

    public void SpawnDefender(GameObject defender)
    {
        if(spawnPosition.childCount == 0)
        {
            var d = Instantiate(defender, spawnPosition.position, Quaternion.identity);
            d.transform.parent = spawnPosition;
            //d.transform.position = spawnPosition.position;
            status = DefenderSpawnPointStatus.Occupied;
        }
    }
}
