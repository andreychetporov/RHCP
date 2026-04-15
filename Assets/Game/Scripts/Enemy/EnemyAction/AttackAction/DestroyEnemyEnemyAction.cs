using UnityEngine;
using Game.Enemy;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Attack")]
    public class DestroyEnemyEnemyAction : EnemyAction
    {
        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            if (owner == null)
            {
                Status = ActionStatus.Failure;
                return;
            }

            EnemyController enemyController = owner.GetComponent<EnemyController>();

            if (enemyController == null)
            {
                Debug.LogWarning($"EnemyController not found");
                Status = ActionStatus.Failure;
                return;
            }

            HealthPointController healthController = enemyController.HealthController;

            if (healthController == null)
            {
                Debug.LogWarning($"HealthController not found");
                Status = ActionStatus.Failure;
                return;
            }

            int currentHealth = healthController.Health.Value;

            if (currentHealth > 0)
            {
                healthController.TakeDamage(currentHealth);
            }

            owner.TargetVelocity = Vector3.zero;
            owner.TargetAngularVelocity = Vector3.zero;

            Status = ActionStatus.Success;
        }

    }
}