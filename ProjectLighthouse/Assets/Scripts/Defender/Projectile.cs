using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 500f;
    [SerializeField] float damage = 1f;
    Rigidbody rb;

    private Transform target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Setup(float damage, float bulletSpeed = 500f)
    {
        this.damage = damage;
        this.bulletSpeed = bulletSpeed;
    }

    private void Update()
    {
        //if (target != null)
        //    rb.position = Vector3.MoveTowards(rb.position, target.position, bulletSpeed * Time.deltaTime);
        //else
        //    rb.position = Vector3.MoveTowards(rb.position, transform.forward, bulletSpeed * Time.deltaTime);
    }

    public void Fire(Vector3 direction, Transform targetTrans)
    {
        target = targetTrans;
        rb.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy) && !enemy.IsDead())
        {
            enemy.ReceiveDamage(damage);
        }
        Destroy(gameObject);
    }
}
