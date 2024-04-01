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
        currentTime = 0;
        rb.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.TryGetComponent(out Enemy enemy) && !enemy.IsDead())
        {
            enemy.ReceiveDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
