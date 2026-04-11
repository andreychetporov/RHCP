using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Movement/Jump")]
    public class JumpEnemyAction : EnemyAction
    {
        [Header("Jump Settings")]
        [SerializeField] private float _jumpForce = 10.0f;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            owner.TargetVelocity = new Vector3(owner.TargetVelocity.x, _jumpForce, owner.TargetVelocity.z);
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            if (owner.TargetVelocity.y <= 0f && owner.IsGrounded)
            {
                Status = ActionStatus.Success;
            }
        }
    }
}