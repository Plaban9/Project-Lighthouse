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

        _agent.SetDestination(_lightHouse.position);
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

        transform.LookAt(_lightHouse);

        if(Physics.CheckSphere(transform.position, 25, _lightHouse.gameObject.layer))
        {
            GameManager.Instance.SetGameOver(true);
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
