using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;

public class Enemy : MonoBehaviour
{
    [SerializeField] float maxHp = 10f;
    [SerializeField] ReactiveProperty<float> curHp = new ReactiveProperty<float>();
    [SerializeField] private Animator _anim;
    private NavMeshAgent _agent;
    private Transform _lightHouse;
    Subject<bool> isDead = new Subject<bool>();

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _lightHouse = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        curHp.Value = maxHp;
        _agent.SetDestination(_lightHouse.position);
        transform.LookAt(_lightHouse);

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
        yield return null;
        Destroy(gameObject);
    }
}
