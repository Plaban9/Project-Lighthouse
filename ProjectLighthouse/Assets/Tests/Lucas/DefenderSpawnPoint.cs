using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderSpawnPoint : MonoBehaviour
{
    MeshRenderer baseMesh;

    [SerializeField] Material normalMaterial;
    [SerializeField] Material onSelectMaterial;
    // Start is called before the first frame update

    RaycastHit hitData;
    DefenderSpawnManager DSM;

    void Start()
    {
        baseMesh = GetComponent<MeshRenderer>();
        DSM = DefenderSpawnManager.Instance;
    }

    // Update is called once per frame
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
        if(set && baseMesh.material != onSelectMaterial)
        {
            baseMesh.material = onSelectMaterial;
        }
        else if(!set && baseMesh.material != normalMaterial)
        {
            baseMesh.material = normalMaterial;
        }
    }
}
