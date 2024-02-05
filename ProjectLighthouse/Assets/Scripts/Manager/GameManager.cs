using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{
    public DefenderSpawnManager defenderSpawnManager;
    public EnemySpawnManager enemySpawnManager;

    static GameManager instance;

    ReactiveProperty<bool> isGameOver = new ReactiveProperty<bool>(false);

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();

            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    public ReactiveProperty<bool> SubscribeGameOver() => isGameOver;

    public bool IsGameOver() => isGameOver.Value;

    public void SetGameOver(bool set) => isGameOver.Value = set;

    public void Reset()
    {
        enemySpawnManager.Reset();
        defenderSpawnManager.Reset();
    }
}
