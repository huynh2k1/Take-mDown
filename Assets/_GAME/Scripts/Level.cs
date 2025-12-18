using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Level : MonoBehaviour
{
    private List<Enemy> _enemies;

    private void Awake()
    {
        _enemies = new List<Enemy>();
        _enemies = transform.GetComponentsInChildren<Enemy>().ToList();
    }

    private void OnEnable()
    {
        foreach(var e in _enemies)
        {
            e.OnEnemyTriggerDestroyArea += RemoveEnemy;
        }
    }

    private void OnDestroy()
    {
        foreach (var e in _enemies)
        {
            e.OnEnemyTriggerDestroyArea += RemoveEnemy;
        }
    }

    void RemoveEnemy(Enemy e)
    {
        if(_enemies.Contains(e))
            _enemies.Remove(e);
        if(_enemies.Count == 0)
        {
            GameController.I.WinGame();
        }
    }

}
