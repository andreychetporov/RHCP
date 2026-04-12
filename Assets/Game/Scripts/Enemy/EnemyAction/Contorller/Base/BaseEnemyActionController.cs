using UnityEngine;

namespace Game.Enemy.Action
{
    public abstract class BaseEnemyActionController : MonoBehaviour
    {
        public Vector3 TargetVelocity;
        public Vector3 TargetAngularVelocity;
        public float Gravity = -20.0f;

        public bool IsJumping { get; protected set; }
        public abstract bool IsGrounded { get; }

        protected EnemyActionBehaviorSO _runtimeBehavior;
        protected bool _isFinished;
        
        public virtual void Start()
        {
            if (_runtimeBehavior != null)
            {
                _runtimeBehavior.RootAction.Enter(this);
                _isFinished = false;
            }
        }

        public virtual void Initialize(EnemyActionBehaviorSO so)
        {
            if (so == null)
            {
                _runtimeBehavior = null;
                _isFinished = true;
                return;
            }

            _runtimeBehavior = so.Clone();
            _isFinished = false;
        }

        public void SetTriggerJump() { IsJumping = true; }
        public abstract void SetCollisionData(bool detectCollisions, LayerMask includeLayers, LayerMask excludeLayers);

        public virtual void BehaviorHandle(float dt)
        {
            if (_runtimeBehavior == null || _isFinished)
                return;

            _runtimeBehavior.RootAction.Process(this, dt);

            if (_runtimeBehavior.RootAction.Status != EnemyAction.ActionStatus.Running)
            {
                _runtimeBehavior.RootAction.Exit(this);
                _isFinished = true;
            }
        }

        public virtual void GravityHandle(float dt)
        {
            if (IsJumping && IsGrounded)
            {
                IsJumping = false;
            }

            if (Mathf.Abs(Gravity) <= 0.001f)
                return;

            if (!IsGrounded)
            {
                TargetVelocity.y += Gravity * dt;
            }
            else if (Mathf.Abs(TargetVelocity.y) < 0.05f)
            {
                TargetVelocity.y = -2.0f;
            }
        }
    }
}