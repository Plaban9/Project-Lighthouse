using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighthouseTargetting : MonoBehaviour
{
    [SerializeField]
    List<Enemy> _targettableEnemies = new List<Enemy>();

    public List<Enemy> GetTargettableEnemies()
    {
        return _targettableEnemies;
    }

    public void AddToTargettable(Enemy target)
    {
        _targettableEnemies.Add(target);
    }

    public void RemoveFromTargettable(Enemy target)
    {
        if (_targettableEnemies.Contains(target))
        {
            _targettableEnemies.Remove(target);
        }
    }

    public void FlushEnemies()
    {
        _targettableEnemies.Clear();
    }
}
