using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 500f;
    [SerializeField] float damage = 1f;
    Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Setup(float damage, float bulletSpeed = 500f)
    {
        this.damage = damage;
        this.bulletSpeed = bulletSpeed;
    }

    public void Fire(Vector3 direction)
    {
        rb.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy) && !enemy.IsDead())
        {
            enemy.ReceiveDamage(damage);
        }
        Debug.Log(other.name);
        Destroy(gameObject);
    }
}
