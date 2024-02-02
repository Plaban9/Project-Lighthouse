using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Rigidbody rb;
    float bulletSpeed = 20f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    public void Fire(Vector3 direction)
    {
        rb.AddForce(direction * bulletSpeed);
        Destroy(gameObject, 5f);
    }
}
