using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Movement/Rotate")]
    public class LookAtVelocityEnemyAction : EnemyAction
    {
        [Header("Settings")]
        [SerializeField] private float _turnSpeed = 8.0f;
        [SerializeField] private float _maxAngularVel = 15.0f;
        [SerializeField] private float _damping = 6.0f;
        [SerializeField] private float _waitTime = 0.05f;
        [SerializeField] private float _stopThreshold = 0.01f;

        [System.NonSerialized] private float _currentTime = 0.0f;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);
            _currentTime = 0.0f;
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            Vector3 horizontalVel = new Vector3(owner.TargetVelocity.x, 0.0f, owner.TargetVelocity.z);
            
            if (horizontalVel.sqrMagnitude < _stopThreshold && Mathf.Abs(owner.TargetVelocity.y) < 2.5f)
            {
                _currentTime += dt;
                if (_currentTime >= _waitTime)
                {
                    Status = ActionStatus.Success;
                    owner.TargetAngularVelocity = Vector3.zero;
                }
                owner.TargetAngularVelocity *= Mathf.Exp(-_damping * dt);
                return;
            }

            _currentTime = 0.0f;

            Quaternion targetRot = Quaternion.LookRotation(owner.TargetVelocity.normalized, Vector3.up) * Quaternion.Euler(0, -90.0f, 0);

            Quaternion delta = targetRot * Quaternion.Inverse(owner.transform.rotation);
            delta.ToAngleAxis(out float angle, out Vector3 axis);

            if (Mathf.Abs(angle) < 1.0f)
            {
                owner.TargetAngularVelocity *= Mathf.Exp(-_damping * dt);
                return;
            }

            Vector3 desiredAngVel = axis * angle * Mathf.Deg2Rad * _turnSpeed;
            float t = 1.0f - Mathf.Exp(-_damping * dt);

            owner.TargetAngularVelocity = Vector3.Lerp(owner.TargetAngularVelocity, desiredAngVel, t);
            owner.TargetAngularVelocity = Vector3.ClampMagnitude(owner.TargetAngularVelocity, _maxAngularVel);
        }
    }
}