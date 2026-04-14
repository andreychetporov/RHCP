using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Enemy
{
    public class EnemyFactory : IEnemyFactory
    {
        [Inject] private readonly EnemyController _prefab;

        private List<EnemyController> _pool = new List<EnemyController>();

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

            if (enemy == null) { enemy = GameObject.Instantiate(_prefab, spawnTransform); }
            else { enemy.transform.parent = spawnTransform; }
            enemy.Initialize(enemySO);

            Debug.Log($"Factory {enemy.gameObject}");

            enemy.transform.position = spawnTransform.position;
            enemy.transform.rotation = spawnTransform.rotation;

            return enemy;
        }
    }
}