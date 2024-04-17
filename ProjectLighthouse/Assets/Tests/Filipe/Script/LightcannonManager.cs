using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightcannonManager : MonoBehaviour
{
    [SerializeField] public LighthouseTargetting _lt;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _lt.AddToTargettable(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _lt.RemoveFromTargettable(enemy);
        }
    }
}
