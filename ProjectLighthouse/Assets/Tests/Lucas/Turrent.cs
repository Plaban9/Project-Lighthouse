using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrent : MonoBehaviour
{
    [SerializeField] Transform gunTransform;
    [SerializeField] GameObject bulletPrefab;
    RaycastHit hitInfo;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shot", 1f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(gunTransform.position, gunTransform.TransformDirection(Vector3.forward) * 1000f, Color.green);


    }

    void Shot()
    {
        var bullet = Instantiate(bulletPrefab, gunTransform.position, Quaternion.identity).GetComponent<Projectile>();
        bullet.transform.forward = gunTransform.TransformDirection(Vector3.forward);
        bullet.Fire(Vector3.forward);
    }
}
