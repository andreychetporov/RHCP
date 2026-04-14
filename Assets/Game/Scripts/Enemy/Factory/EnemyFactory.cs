using UnityEngine;
using Zenject;

namespace Game.Enemy
{
    public class EnemyFactory : IEnemyFactory
    {
        [Inject] private readonly EnemyController _prefab;

        public EnemyController CreateEnemy(EnemySO enemySO, Transform spawnTransform)
        {
            EnemyController enemy = GameObject.Instantiate(_prefab, spawnTransform.position, spawnTransform.rotation);
            enemy.Initialize(enemySO);

            return enemy;

        }
    }
}