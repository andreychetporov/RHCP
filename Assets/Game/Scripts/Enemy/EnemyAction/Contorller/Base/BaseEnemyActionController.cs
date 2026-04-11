using UnityEngine;

namespace Game.Enemy.Action
{
    public abstract class BaseEnemyActionController : MonoBehaviour
    {
        public Vector3 TargetVelocity;
        public Vector3 TargetAngularVelocity;
        public float Gravity = -20.0f;

        public abstract bool IsGrounded { get; }

        protected EnemyActionBehaviorSO _runtimeBehavior;
        protected bool _isFinished = false;

        public virtual void Start() => _runtimeBehavior?.RootAction.Enter(this);

        public virtual void Initialize(EnemyActionBehaviorSO so)
        {
            _runtimeBehavior = so.Clone();
            _runtimeBehavior?.RootAction.Enter(this);
        }

        public abstract void SetCollisionData(bool detectCollisions, LayerMask includeLayers, LayerMask excludeLayers);

        public virtual void BehaviorHandle(float dt)
        {
            if (_runtimeBehavior != null)
            {
                _runtimeBehavior.RootAction.Process(this, Time.deltaTime);

                if (_runtimeBehavior.RootAction.Status != EnemyAction.ActionStatus.Running && !_isFinished)
                {
                    _runtimeBehavior.RootAction.Exit(this);
                    _isFinished = true;
                }
            }
        }

        public virtual void GravityHandle(float dt)
        {
            if (Mathf.Abs(Gravity) > 0.001f)
            {
                if (!IsGrounded)
                {
                    TargetVelocity.y += Gravity * dt;
                }
                else if (TargetVelocity.y < 0)
                {
                    TargetVelocity.y = -2.0f;
                }
            }
        }
    }
}