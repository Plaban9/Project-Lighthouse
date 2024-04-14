using UnityEngine;

[CreateAssetMenu(menuName ="Defender/Defender Data")]
public class DefenderData : ScriptableObject
{
    [Header("Fire Info")]
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float damage;
    public float fireRate;
    public float rotationSpeed;

    [Header("Vision")]
    public int visionRadius;
    public LayerMask targetMask;

    public int cost;
}

//archor
//canon
//poison
//wizard
