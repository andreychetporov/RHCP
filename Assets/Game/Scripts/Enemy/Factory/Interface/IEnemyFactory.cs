using UnityEngine;

namespace Game.Enemy
{
    public interface IEnemyFactory
    {
        public EnemyController CreateEnemy(EnemySO enemySO, Transform spawnTransform);
    }
}
