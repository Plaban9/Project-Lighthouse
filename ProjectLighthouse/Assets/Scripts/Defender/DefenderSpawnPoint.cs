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
    Camera cam;

    void Start()
    {
        baseMesh = GetComponent<MeshRenderer>();
        DSM = DefenderSpawnManager.Instance;
        cam = Camera.main;

        if(spawnPosition == null)
        {
            spawnPosition = transform.Find("SpawnPosition");
        }
    }

    void Update()
    {
        // Make All White
        if (baseMesh.material.color != Color.white)
            baseMesh.material.color = Color.white;

        if (!DSM.IsDraggingDefenderFromMenu())
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        bool currentSelectedSlot = Physics.Raycast(ray, out hitData, 1000) && hitData.transform == transform;

        if (currentSelectedSlot)
        {
            baseMesh.material.color = Color.green; // We selecting current gameobject
        }
        else if (status == DefenderSpawnPointStatus.Occupied)
        {
            baseMesh.material.color = Color.red; // It is already occupied
        }
    }

    public void SpawnDefender(GameObject defender)
    {
        if(spawnPosition.childCount == 0)
        {
            var d = Instantiate(defender, spawnPosition.position, Quaternion.identity);
            d.transform.parent = spawnPosition;
            d.transform.rotation = Quaternion.identity;
            d.GetComponent<DefenderObject>().SetDeployed(true);

            //d.transform.position = spawnPosition.position;
            status = DefenderSpawnPointStatus.Occupied;
        }
    }

    public void Reset()
    {
        if(spawnPosition.childCount > 0)
        {
            foreach(Transform go in spawnPosition)
            {
                Destroy(go.gameObject);
            }
        }

        status = DefenderSpawnPointStatus.Available;
    }
}
