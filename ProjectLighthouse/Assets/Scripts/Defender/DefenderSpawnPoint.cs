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
    [SerializeField] MeshRenderer baseMesh;
    Color originalColor;
    [Header("Spawning")]
    [SerializeField] Transform spawnPosition;
    [SerializeField] float spawnScale = 3f;

    [Header("Particle Effect")]
    [SerializeField] private ParticleSystem spawnEffect;

    DefenderSpawnPointStatus status;
    RaycastHit hitData;
    DefenderSpawnManager DSM;
    Camera cam;

    void Start()
    {
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
        if (baseMesh.material.color != originalColor)
        {
            baseMesh.material.color = originalColor;
        }

        if (!DSM.IsDraggingDefenderFromMenu())
            return;

        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);  // Rather than making spawnpoint do all calculation (every frame)
        Ray ray = DSM.GetMousePositionRelativeToScreen();       // We can use DSM to do it for us just once (every frame)
        bool currentSelectedSlot = Physics.Raycast(ray, out hitData, 1000) && hitData.transform == baseMesh.transform;
        if (currentSelectedSlot && status == DefenderSpawnPointStatus.Available)
        {
            baseMesh.material.color = Color.green; // We selecting current gameobject
        }
        else if (currentSelectedSlot && status == DefenderSpawnPointStatus.Occupied)
        {
            baseMesh.material.color = Color.red; // It is already occupied
        }
    }

    public void SpawnDefender(GameObject defender)
    {
        if(spawnPosition.childCount == 0)
        {
            var d = Instantiate(defender, spawnPosition.position, Quaternion.identity).GetComponent<DefenderObject>();
            d.transform.DOScale(spawnScale, 0.3f).OnComplete(() => {
                d.transform.parent = spawnPosition;
                d.transform.rotation = Quaternion.identity;
                d.SetDeployed(true);
            });

            //d.transform.position = spawnPosition.position;
            status = DefenderSpawnPointStatus.Occupied;
            spawnEffect.Play();
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
