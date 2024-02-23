using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
public class DefenderObject : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] Transform gunRotationPart;
    [SerializeField] List<Transform> gunPoints = new List<Transform>();
    [SerializeField] List<Transform> gunPoints2 = new List<Transform>();

    Animator animator;

    [Header("Fire Info")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate = 0.2f;
    [SerializeField] float rotationSpeed = 150f;
    private float fireTimer = 0f;
    private float fireTimer2 = 0f;
    private int curGunPointIndex = 0;
    private int curGunPointIndex2 = 0;

    [Header("Vision")]
    [SerializeField] Transform vision;
    [SerializeField] float visionRadius = 10f;
    [SerializeField] LayerMask targetMask;

    [Header("Target Enemy")]
    [SerializeField] Enemy targetEnemy = null;
    RaycastHit hitInfo;
    bool isDeployed = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 1 / fireRate;
    }

    private void Start()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(vision.position, visionRadius);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isDeployed) return;

        var encounterEnemy = Physics.OverlapSphere(vision.position, visionRadius, targetMask);

        if(encounterEnemy.Length > 0)
        {
            if (targetEnemy == null || targetEnemy.IsDead())
            {
                var search = encounterEnemy.FirstOrDefault(x => !x.GetComponent<Enemy>().IsDead());
                var newTarget = search != null ? search.GetComponent<Enemy>() : null;

                if (newTarget != null)
                {
                    targetEnemy = newTarget;

                    targetEnemy.SubscribeDeadEvent().Subscribe(dead =>
                    {
                        targetEnemy = null;
                    }).AddTo(targetEnemy.gameObject);
                }
            }
        }

        foreach (var gunPoint in gunPoints)
            Debug.DrawRay(gunPoint.position, gunPoint.forward * visionRadius, Color.green);

        foreach (var gunPoint in gunPoints2)
            Debug.DrawRay(gunPoint.position, gunPoint.forward * visionRadius, Color.green);

        if (targetEnemy != null)
        {
            var direction = (targetEnemy.transform.position - gunRotationPart.position).normalized;
            var targetRotation = Quaternion.LookRotation(direction);

           
            gunRotationPart.rotation = Quaternion.RotateTowards(gunRotationPart.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            foreach (var gunPoint in gunPoints)
            {
                if(Physics.Raycast(gunPoint.position, gunPoint.forward, out hitInfo, visionRadius, targetMask))
                {
                    if (hitInfo.transform.TryGetComponent(out Enemy enemy))
                    {
                        fireTimer += Time.deltaTime;

                        if (fireTimer >= fireRate)
                        {
                            Shot();
                        }

                        break;
                    }
                }
            }
            foreach (var gunPoint in gunPoints2)
            {
                if(Physics.Raycast(gunPoint.position, gunPoint.forward, out hitInfo, visionRadius, targetMask))
                {
                    if (hitInfo.transform.TryGetComponent(out Enemy enemy))
                    {
                        fireTimer2 += Time.deltaTime;

                        if (fireTimer2 >= fireRate)
                        {
                            Shot2();
                        }

                        break;
                    }
                }
            }
        }
    }

    public void SetDeployed(bool set)
    {
        isDeployed = set;
    }

    void Shot()
    {
        animator.SetTrigger("Shoot");
        var gunPoint = gunPoints[curGunPointIndex++ % gunPoints.Count];
        var bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation).GetComponent<Projectile>();
        bullet.Fire(gunPoint.forward);

        fireTimer = 0f;
    }
    void Shot2()
    {
        animator.SetTrigger("Shoot");
        var gunPoint = gunPoints2[curGunPointIndex2++ % gunPoints2.Count];
        var bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation).GetComponent<Projectile>();
        bullet.Fire(gunPoint.forward);

        fireTimer2 = 0f;
    }

}
