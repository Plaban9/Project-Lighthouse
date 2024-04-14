using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    [SerializeField] float maxHp = 10f;
    [SerializeField] ReactiveProperty<float> curHp = new ReactiveProperty<float>();
    [SerializeField] private Animator _anim;
    [SerializeField] int dropCoins = 100;
    private NavMeshAgent _agent;
    private Transform _lightHouse;
    Subject<bool> isDead = new Subject<bool>();

    private void Awake()
    {
        _agent = GetComponentInChildren<NavMeshAgent>();
        _lightHouse = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        curHp.Value = maxHp;
        _agent.SetDestination(_lightHouse.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameOver()) return;

        //transform.LookAt(_lightHouse);

        if (Physics.CheckSphere(transform.position, 25, _lightHouse.gameObject.layer))
        {
            GameManager.Instance.SetGameOver(true);
        }
    }

    private void FixedUpdate()
    {
        float velocity = _agent.velocity.magnitude / _agent.speed;

        if (velocity == 0.0f)
        {
            Idle();
        }
        else
        {
            Walk();
        }
    }

    public void Setup(float hp)
    {
        maxHp = hp;
        curHp.Value = hp;
    }

    public void ReceiveDamage(float dmg)
    {
        //GameManager.Instance.SetGameOver(true);
        if (curHp.Value <= 0) return;

        curHp.Value -= dmg;

        if (curHp.Value <= 0)
        {
            CurrencyManager.Instance.AddCoin(dropCoins);

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Gameover"))
        {
            GameManager.Instance.SetGameOver(true);
        }
    }

    #region Animations

    //Attack anim
    private void Attack()
    {
        _anim.SetTrigger("Attack");
    }

    //Hurt anim
    private void Hurt()
    {
        _anim.SetTrigger("Hurt");
    }


    private void Idle()
    {
        _anim.SetBool("Walk", false);
    }

    private void Walk()
    {
        _anim.SetBool("Walk", true);
    }

    private void Die()
    {
        _anim.SetTrigger("Die");
    }
    #endregion
}
