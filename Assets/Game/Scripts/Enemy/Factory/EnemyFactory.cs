using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Enemy
{
    public class EnemyFactory : IEnemyFactory
    {
        private EnemyController _prefab;

        private List<EnemyController> _pool = new List<EnemyController>();

        private Transform _container;

        [Inject]
        private void Initialize(EnemyController prefab)
        {
            _prefab = prefab;

            _container = new GameObject().transform;
            _container.name = "CONTAINER_EnemyFactory";

            for (int i = 0; i < 10; i++)
            {
                var e = GameObject.Instantiate(_prefab, _container);
                e.gameObject.SetActive(false);
                _pool.Add(e);
            }
        }

        public EnemyController CreateEnemy(EnemySO enemySO, Transform spawnTransform)
        {
            EnemyController enemy = null;
            foreach (EnemyController controller in _pool)
            {
                if (controller != null && !controller.gameObject.activeSelf)
                {
                    enemy = controller;
                    break;
                }
            }

            if (enemy == null)
            {
                enemy = GameObject.Instantiate(_prefab, spawnTransform.position, spawnTransform.rotation, _container);
            }
            else
            {
                enemy.transform.position = spawnTransform.position;
                enemy.transform.rotation = spawnTransform.rotation;
            }

            enemy.Initialize(enemySO);
            enemy.gameObject.SetActive(true);

            Debug.Log($"Factory {enemy.gameObject}");

            return enemy;
        }
    }
}