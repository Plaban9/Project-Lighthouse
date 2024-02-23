using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum DefenderSpawnPointStatus
{
    Available,
    Occupied
}

public class DefenderSpawnPoint : MonoBehaviour
{
    MeshRenderer baseMesh;
    Color originalColor;
    [Header("Spawning")]
    [SerializeField] Transform spawnPosition;

    DefenderSpawnPointStatus status;
    RaycastHit hitData;
    DefenderSpawnManager DSM;
    Camera cam;

    void Start()
    {
        baseMesh = GetComponent<MeshRenderer>();
        originalColor = baseMesh.material.color;
        DSM = DefenderSpawnManager.Instance;
        cam = Camera.main;
        status = DefenderSpawnPointStatus.Available;

        if (spawnPosition == null)
        {
            spawnPosition = transform.Find("SpawnPosition");
        }
    }

    void Update()
    {
        if (!DSM.IsDraggingDefenderFromMenu())
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        bool currentSelectedSlot = Physics.Raycast(ray, out hitData, 1000) && hitData.transform == transform;
        if (currentSelectedSlot && status == DefenderSpawnPointStatus.Available)
        {
            baseMesh.material.color = Color.green; // We selecting current gameobject
        }
        else if (currentSelectedSlot && status == DefenderSpawnPointStatus.Occupied)
        {
            baseMesh.material.color = Color.red; // It is already occupied
        }
        else 
        {
            if (baseMesh.material.color != originalColor) 
            {
                baseMesh.material.color = originalColor;
            }
        }
    }

    public void SpawnDefender(GameObject defender)
    {
        if(spawnPosition.childCount == 0)
        {
            var d = Instantiate(defender, spawnPosition.position, Quaternion.identity).GetComponent<DefenderObject>();
            d.transform.DOScale(3.0f, 0.3f).OnComplete(() => {
                d.transform.parent = spawnPosition;
                d.transform.rotation = Quaternion.identity;
                d.SetDeployed(true);
            });

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
