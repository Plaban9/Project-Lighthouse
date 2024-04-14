using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{

    [SerializeField] private Animator _anim;
    [SerializeField] private EnemyHealthBar _healthBar;
    [SerializeField] int dropCoins = 100;
    private NavMeshAgent _agent;
    private Transform _lightHouse;
    Subject<bool> isDead = new Subject<bool>();
    private EnemySpawnManager _manager;

    [Header("Attributes")]
    [SerializeField] float maxHp = 10f;
     ReactiveProperty<float> curHp = new ReactiveProperty<float>();
    [SerializeField] public float _attackDamage = 10f;
    [SerializeField] public float _attackDelay = 5f;
    private float _attackCooldown = 0f;
    [SerializeField] public float _movespeed = 1f;



    private void Awake()
    {
        _agent = GetComponentInChildren<NavMeshAgent>();
        _lightHouse = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        curHp.Value = maxHp;
        _agent.speed = _agent.speed * _movespeed;
        _agent.SetDestination(_lightHouse.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameOver()) return;

        //transform.LookAt(_lightHouse);

        if(_attackCooldown > 0f)
        {
            _attackCooldown -= Time.deltaTime;
        }

        Vector3 ignoreYe = transform.position;
        ignoreYe.y = 0;
        Vector3 ignoreYl = _lightHouse.position;
        ignoreYl.y = 0;
        if (Vector3.Distance(ignoreYe, ignoreYl) <= 30f)
        {
            _agent.speed = 0f;
            if(_attackCooldown <= 0f)
            {
                _attackCooldown = _attackDelay;
                Attack();
            }
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
        _healthBar.SetPercentHP(curHp.Value / maxHp);


    }

    public void ReceiveDamage(float dmg)
    {
        //GameManager.Instance.SetGameOver(true);
        if (curHp.Value <= 0) return;

        curHp.Value -= dmg;
        _healthBar.SetPercentHP(curHp.Value / maxHp);
        Hurt();

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

    public void SetManager(EnemySpawnManager manager)
    {
        _manager = manager;
    }

    IEnumerator PerformDead()
    {
        _manager.RemoveEnemy(this.gameObject);
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
        GameManager.Instance.LighthouseHp.TakeDamage(_attackDamage);
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
