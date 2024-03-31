using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefenderSpawnManager : MonoBehaviour
{
    static DefenderSpawnManager instance;

    bool isDraggingDefenderFromMenu = false;

    [SerializeField] List<DefenderSpawnPoint> defenderSPList = new List<DefenderSpawnPoint>();

    [SerializeField] List<RockSpawner> _rockSpawners = new List<RockSpawner>();

    private Camera _camera;

    private void OnEnable()
    {
        TimeController.dayNightCycleStartNotifier += DayNightEventHandler;
    }

    private void OnDisable()
    {
        TimeController.dayNightCycleStartNotifier -= DayNightEventHandler;
    }

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
    }

    private void Start()
    {
        Init();
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

    public void Init()
    {
        _camera = Camera.main;

        foreach (var rock in _rockSpawners)
        {
            rock.SpawnRock();
        }

        defenderSPList = FindObjectsByType<DefenderSpawnPoint>(FindObjectsSortMode.None).ToList(); // More safe
        //defenderSPList = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(x => x.GetComponent<DefenderSpawnPoint>()).ToList();
    }

    public Ray GetMousePositionRelativeToScreen()
    {
        return _camera.ScreenPointToRay(Input.mousePosition);
    }

    public void SetIsDraggingDefenderFromMenu(bool set)
    {
        isDraggingDefenderFromMenu = set;
    }

    public bool IsDraggingDefenderFromMenu() => isDraggingDefenderFromMenu;


    private void DayNightEventHandler(DayNightCycle dayNightCycle)
    {
        switch (dayNightCycle)
        {
            case DayNightCycle.DAY:
                OnDayStarted();
                break;
            case DayNightCycle.NIGHT:
                OnNightStarted();
                break;
        }
    }

    private void OnDayStarted()
    {
        d("Day Started");
        //foreach (var rock in _rockSpawners)
        {
            //rock.SpawnRock();
        }

        //defenderSPList = FindObjectsByType<DefenderSpawnPoint>(FindObjectsSortMode.None).ToList(); // More safe
        //defenderSPList = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(x => x.GetComponent<DefenderSpawnPoint>()).ToList();
    }

    private void OnNightStarted()
    {
        d("Night Started");
    }

    private static void d(string message)
    {
        Debug.Log("<<DefenderSpawnManager>> " + message);
    }
}
