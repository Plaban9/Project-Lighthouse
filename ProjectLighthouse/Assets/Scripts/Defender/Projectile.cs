using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 500f;
    [SerializeField] float damage = 1f;
    Rigidbody rb;

    float currentTime = 0f;
    float maxlifeTime = 10f;

    /// <summary>
    /// Tracking stuff!
    /// </summary>
    private GameObject _trackedEnemy = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Setup(float damage, float bulletSpeed = 500f, GameObject trackedEnemy = null)
    {
        this.damage = damage;
        this.bulletSpeed = bulletSpeed;
        this._trackedEnemy = trackedEnemy;
    }

    private void Update()
    {
        if(this._trackedEnemy != null)
        {
            Vector3 direction = (_trackedEnemy.transform.position - transform.position).normalized;
            transform.position += direction * bulletSpeed * Time.deltaTime;
            if (!this._trackedEnemy.activeSelf)
            {
                StartCoroutine(KillAfter(1f));
            }
        }
        if(currentTime < maxlifeTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Fire(Vector3 direction)
    {
        _trackedEnemy = null;
            currentTime = 0;
            rb.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
    }

    public void TrackingFire( GameObject target)
    {
        _trackedEnemy = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy) && !enemy.IsDead())
        {
            enemy.ReceiveDamage(damage);
            StartCoroutine(KillAfter(1f));
        }
    }

    private IEnumerator KillAfter(float timetodeath)
    {
        float elapsed = timetodeath;
        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
