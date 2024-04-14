using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{
    public DefenderSpawnManager defenderSpawnManager;
    public EnemySpawnManager enemySpawnManager;
    public TimeController timeController;
    public LighthouseHandler lighthouseHandler;
    public LighthouseHp LighthouseHp;

    static GameManager instance;

    ReactiveProperty<bool> isGameOver = new ReactiveProperty<bool>(false);

    [Header("Gameplay")]
    public float timeToStartGame = 10f;
    public float dayTime = 30f;
    public List<int> enemyCountWave;

    private bool _isGameRunning = false;
    private int _waves;
    private int _waveIndex = 0;



    public static GameManager Instance => instance;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public ReactiveProperty<bool> SubscribeGameOver() => isGameOver;

    public bool IsGameOver() => isGameOver.Value;

    public void SetGameOver(bool set) => isGameOver.Value = set;

    public void Reset()
    {
        enemySpawnManager.Reset();
        defenderSpawnManager.Reset();

        isGameOver.Value = false;
    }


    public void Update()
    {
        if(timeToStartGame > 0f)
        {
            timeToStartGame -= Time.deltaTime;
        }
        else if(!_isGameRunning)
        {
            _waves = enemyCountWave.Count;
            _isGameRunning = true;
            DayStart();
        }
    }

    public void DayStart()
    {
        timeController.MoveToTime(8);
        if(_waveIndex >= _waves)
        {
            SetGameOver(true);
            return;
        }
        StartCoroutine(DaytimeRoutine());
    }
        
    public IEnumerator DaytimeRoutine()
    {
        float timeToWarning = dayTime * 0.75f;

        while(timeToWarning > 0f)
        {
            timeToWarning -= Time.deltaTime;
            yield return null;
        }
        lighthouseHandler.Warning("NIGHT IS APPROACHING...");
        timeToWarning = dayTime * 0.25f;
        while (timeToWarning > 0f)
        {
            timeToWarning -= Time.deltaTime;
            yield return null;
        }
        NightStart();
    }

    public void NightStart()
    {
        int enemiesToSpawn = enemyCountWave[_waveIndex];
        enemySpawnManager.SetNumberOfEnemiesToSpawn(enemiesToSpawn);
        timeController.MoveToTime(20);
    }

    public void NightEnd()
    {
        _waveIndex++;
        DayStart();
    }

    public bool GetGameStarted()
    {
        return _isGameRunning;
    }
}
