using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    public class JumpEnemyAction : EnemyAction
    {
        [Header("Jump Settings")]
        [SerializeField] private float _jumpForce = 10.0f;

        public override void Enter(EnemyActionController owner)
        {
            base.Enter(owner);

            owner.Velocity = new Vector3(owner.Velocity.x, _jumpForce, owner.Velocity.z);
        }

        public override void Process(EnemyActionController owner, float dt)
        {
            if (owner.Velocity.y <= 0f && owner.IsGrounded)
            {
                Status = ActionStatus.Success;
            }
        }
    }
}