using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Movement/Jump")]
    public class AutoJumpEnemyAction : EnemyAction
    {
        [Header("Detection Settings")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _checkDistance = 1.0f;
        [SerializeField] private float _maxJumpHeight = 1.5f;
        [SerializeField] private float _maxSafeFallDepth = 3.0f;

        [Header("Jump Physics")]
        [SerializeField] private float _jumpForce = 7.0f;
        [SerializeField] private float _forwardJumpImpulse = 2.0f;

        [System.NonSerialized] private bool _isJump = false;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            _isJump = false;
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            if (_isJump)
            {
                if (!owner.IsJumping && owner.IsGrounded)
                {
                    Status = ActionStatus.Success;
                }

                return;
            }

            if (!owner.IsGrounded) { return; }

            Vector3 origin = owner.transform.position + Vector3.up * 0.1f;
            Vector3 forward = owner.transform.right;

            if (Physics.Raycast(origin, forward, out RaycastHit wallHit, _checkDistance, _groundLayer))
            {
                Vector3 highOrigin = origin + Vector3.up * _maxJumpHeight;
                if (!Physics.Raycast(highOrigin, forward, _checkDistance, _groundLayer))
                {
                    DoJump(owner);
                    return;
                }
            }

            Vector3 edgeCheckPos = origin + forward * _checkDistance;

            if (!Physics.Raycast(edgeCheckPos, Vector3.down, out RaycastHit groundHit, _maxSafeFallDepth, _groundLayer))
            {
                DoJump(owner);
                return;
            }
        }

        private void DoJump(BaseEnemyActionController owner)
        {
            Vector3 currentVel = owner.TargetVelocity;
            currentVel.y = _jumpForce;

            currentVel += owner.transform.forward * _forwardJumpImpulse;

            owner.TargetVelocity = currentVel;

            _isJump = true;
        }
    }
}