using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    public class SetGravityEnemyAction : EnemyAction
    {
        [Header("Settings")]
        [SerializeField] private float _gravityValue = -9.81f;

        public override void Enter(EnemyActionController owner)
        {
            base.Enter(owner);

            owner.Gravity = _gravityValue;

            Status = ActionStatus.Success;
        }
    }
}