using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Enemy.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private List<EnemySO> _spawnedPool;

        [Header("Spawn Settings")]
        [SerializeField] private int _waveCount = 5;
        [SerializeField] private float _initialDelay = 0.5f;
        [SerializeField] private float _spawnInterval = 2.0f;
        [SerializeField] private bool _isInfinite = false;

        [Inject] private readonly IEnemyFactory _enemyFactory;

        private Coroutine _spawnRoutine;

        public void StartSpawning()
        {
            if (_spawnRoutine != null) { return; }

            _spawnRoutine = StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            if (_initialDelay > 0) { yield return new WaitForSeconds(_initialDelay); }
            
            int currentWave = 0;

            while (_isInfinite || currentWave < _waveCount)
            {
                foreach (Transform t in _spawnPoints)
                {
                    _enemyFactory.CreateEnemy(_spawnedPool[Random.Range(0, _spawnedPool.Count)], t);
                }

                currentWave++;

                yield return new WaitForSeconds(_spawnInterval);
            }

            _spawnRoutine = null;
        }

        public void StopSpawning()
        {
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);

                _spawnRoutine = null;
            }
        }
    }
}