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
        rb.AddForce(direction * bulletSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(gameObject);
        if(collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();

            if(enemy != null && !enemy.IsDead())
            {
                enemy.ReceiveDamage(damage);
            }
        }

        Destroy(gameObject);
    }

}
