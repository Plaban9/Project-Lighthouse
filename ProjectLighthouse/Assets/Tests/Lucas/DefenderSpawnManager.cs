using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DefenderSpawnManager : MonoBehaviour
{
    static DefenderSpawnManager instance;

    bool isDraggingDefenderFromMenu = false;

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

    }

    public void SetIsDraggingDefenderFromMenu(bool set)
    {
        isDraggingDefenderFromMenu = set;
    }

    public bool IsDraggingDefenderFromMenu() => isDraggingDefenderFromMenu;
}
