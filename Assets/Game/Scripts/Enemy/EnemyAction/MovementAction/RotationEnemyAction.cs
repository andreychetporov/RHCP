using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Movement/Rotate")]
    public class RotationEnemyAction : EnemyAction
    {
        [Header("Settings")]
        [SerializeField] private Vector3 _angleDelta = new Vector3(0.0f, 90.0f, 0.0f);
        [SerializeField] private float _duration = 1.0f;

        [System.NonSerialized] private float _elapsedTime;
        [System.NonSerialized] private Quaternion _targetAngle;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            _elapsedTime = 0.0f;

            if (_duration <= 0.0f)
            {
                owner.transform.Rotate(_angleDelta, Space.Self);
                Status = ActionStatus.Success;
                return;
            }

            owner.TargetAngularVelocity = (_angleDelta * Mathf.Deg2Rad) / _duration;
            _targetAngle = owner.transform.rotation * Quaternion.Euler(_angleDelta);
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            _elapsedTime += dt;

            if (_elapsedTime >= _duration)
            {
                owner.TargetAngularVelocity = Vector3.zero;
                Status = ActionStatus.Success;
            }
        }

        public override void Exit(BaseEnemyActionController owner)
        {
            owner.TargetAngularVelocity = Vector3.zero;
            owner.transform.rotation = _targetAngle;
        }
    }
}