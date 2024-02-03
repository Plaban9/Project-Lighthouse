using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Enemy : MonoBehaviour
{
    [SerializeField] float maxHp = 10f;
    [SerializeField] ReactiveProperty<float> curHp = new ReactiveProperty<float>();

    Subject<bool> isDead = new Subject<bool>();

    void Start()
    {
        curHp.Value = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(float hp)
    {
        maxHp = hp;
        curHp.Value = hp;
    }

    public void ReceiveDamage(float dmg)
    {
        curHp.Value -= dmg;

        if(curHp.Value <= 0)
        {
            isDead.OnNext(true);
            isDead.Dispose();
            StartCoroutine(PerformDead());
        }
    }

    public Subject<bool> SubscribeDeadEvent()
    {
        return isDead;
    }

    public bool IsDead()
    {
        return curHp.Value <= 0;
    }

    IEnumerator PerformDead()
    {
        Destroy(gameObject);
        yield return null;
    }
}
