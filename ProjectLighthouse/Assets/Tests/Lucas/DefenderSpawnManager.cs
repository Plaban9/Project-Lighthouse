using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class DefenderSpawnManager : MonoBehaviour
{
    static DefenderSpawnManager instance;

    bool isDraggingDefenderFromMenu = false;

    public GameObject testObj;

    public static DefenderSpawnManager Instance
    {
        get
        {
            if (instance == null)
                instance = new DefenderSpawnManager();

            return instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit RayHit;

        if (Physics.Raycast(ray, out RayHit))

        {
            var ObjectHit = RayHit.transform.gameObject;
            var Hitpoint = RayHit.point;

            if (ObjectHit != null && !EventSystem.current.IsPointerOverGameObject())
            {
                Debug.DrawLine(Camera.main.transform.position, Hitpoint, Color.blue, 0.5f);
                //testObj.transform.position = Hitpoint;
            }

        }

    }

    public void SetIsDraggingDefenderFromMenu(bool set)
    {
        isDraggingDefenderFromMenu = set;
    }

    public bool IsDraggingDefenderFromMenu() => isDraggingDefenderFromMenu;
}
