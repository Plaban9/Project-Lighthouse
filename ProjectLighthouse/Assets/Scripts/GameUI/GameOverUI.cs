using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameOverUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SubscribeGameOver().Subscribe(x =>
        {
            gameObject.SetActive(true);
        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickRetry()
    {
        gameObject.SetActive(false);
        GameManager.Instance.Reset();
    }
}
