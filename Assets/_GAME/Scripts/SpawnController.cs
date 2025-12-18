using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] Transform _parent;
    [SerializeField] Enemy _enemyPrefab;
    [SerializeField] Obstacle[] _obstacles;
    [SerializeField] Transform[] _listPos;
    [SerializeField] float _delaySpawn = 1f;

    Coroutine _spawnCoroutine;
    int _currentWave = 0;
    int _maxWave = 0;
    int _enemyCount;

    public event Action OnAllEnemyDeadEvent;

    public void StartSpawn()
    {
        _currentWave = 0;
        _maxWave = 5;
        ClearWave();
        StopSpawnRoutine();
        _spawnCoroutine = StartCoroutine(SpawnRoutine(_maxWave));
    }

    void StopSpawnRoutine()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    IEnumerator SpawnRoutine(int numberWave)
    {
        while (_currentWave < numberWave)
        {
            while(GameController.I.CurState != GameController.State.PLAYING)
                yield return null;
            // Random số object spawn: 1 hoặc 2
            int spawnCount = UnityEngine.Random.Range(1, 3);

            // Danh sách vị trí còn trống
            List<int> availableIndexes = new List<int>();
            for (int i = 0; i < _listPos.Length; i++)
                availableIndexes.Add(i);

            for (int i = 0; i < spawnCount && availableIndexes.Count > 0; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableIndexes.Count);
                int posIndex = availableIndexes[randomIndex];
                Transform spawnPos = _listPos[posIndex];

                // Random Enemy hoặc Obstacle
                bool spawnEnemy = (_currentWave == numberWave - 1) ? true : UnityEngine.Random.value < 0.7f;

                if (spawnEnemy)
                {
                    Enemy e = Instantiate(_enemyPrefab, _parent);
                    e.transform.position = spawnPos.position;
                    _enemyCount++;

                    //e.OnEnemyTriggerDestroyArea += () => OnEnemyDead(e);
                }
                else
                {
                    if (_obstacles.Length > 0)
                    {
                        Obstacle obsPrefab =
                            _obstacles[UnityEngine.Random.Range(0, _obstacles.Length)];
                        Obstacle obs = Instantiate(obsPrefab, _parent);
                        obs.transform.position = spawnPos.position;
                    }
                }

                availableIndexes.RemoveAt(randomIndex);
            }

            _currentWave++;
            yield return new WaitForSeconds(_delaySpawn);
        }

        _spawnCoroutine = null;
    }

    public void ClearWave()
    {
        _enemyCount = 0;
        for (int i = _parent.childCount - 1; i >= 0; i--)
        {
            Destroy(_parent.GetChild(i).gameObject);
        }
    }

    void OnEnemyDead(Enemy enemy)
    {
        //enemy.OnEnemyTriggerDestroyArea -= () => OnEnemyDead(enemy); // tránh leak

        _enemyCount--;

        if (_enemyCount <= 0 && _currentWave == _maxWave)
        {
            OnAllEnemyDeadEvent?.Invoke();
        }
    }
}
