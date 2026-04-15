using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Movement/Jump")]
    public class JumpEnemyAction : EnemyAction
    {
        [Header("Jump Settings")]
        [SerializeField] private float _jumpForce = 10.0f;

        [System.NonSerialized] private bool _isJumped = false;
        [System.NonSerialized] private bool _wasInAir = false;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            _isJumped = false;
            _wasInAir = false;
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            // 👉 1. Старт прыжка
            if (!_isJumped)
            {
                if (!owner.IsGrounded) return;

                owner.TargetVelocity = new Vector3(owner.TargetVelocity.x, _jumpForce, owner.TargetVelocity.z);
                owner.SetTriggerJump();

                _isJumped = true;
                return;
            }

            // 👉 2. Отслеживаем, что реально оторвались от земли
            if (!owner.IsGrounded)
            {
                _wasInAir = true;
                return;
            }

            // 👉 3. Если были в воздухе и снова на земле → SUCCESS
            if (_wasInAir && owner.IsGrounded)
            {
                Status = ActionStatus.Success;
            }
        }
    }
}