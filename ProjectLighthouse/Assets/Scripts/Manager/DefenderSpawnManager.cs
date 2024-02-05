using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class DefenderSpawnManager : MonoBehaviour
{
    static DefenderSpawnManager instance;

    bool isDraggingDefenderFromMenu = false;

    [SerializeField] List<DefenderSpawnPoint> defenderSPList = new List<DefenderSpawnPoint>();

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

    private void Start()
    {
        defenderSPList = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(x => x.GetComponent<DefenderSpawnPoint>()).ToList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reset()
    {
        foreach (var item in defenderSPList)
        {
            item.Reset();
        }
    }


    public void SetIsDraggingDefenderFromMenu(bool set)
    {
        isDraggingDefenderFromMenu = set;
    }

    public bool IsDraggingDefenderFromMenu() => isDraggingDefenderFromMenu;
}
