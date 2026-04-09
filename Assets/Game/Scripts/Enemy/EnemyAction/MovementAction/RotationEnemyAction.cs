using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    public class RotationEnemyAction : EnemyAction
    {
        [Header("Settings")]
        [SerializeField] private Vector3 _angleDelta = new Vector3(0.0f, 90.0f, 0.0f);
        [SerializeField] private float _duration = 1.0f;

        [System.NonSerialized] private float _elapsedTime;

        public override void Enter(EnemyActionController owner)
        {
            base.Enter(owner);

            _elapsedTime = 0.0f;

            if (_duration <= 0.0f)
            {
                owner.transform.Rotate(_angleDelta, Space.Self);
                Status = ActionStatus.Success;
                return;
            }

            owner.AngularVelocity = _angleDelta / _duration;
        }

        public override void Process(EnemyActionController owner, float dt)
        {
            _elapsedTime += dt;

            if (_elapsedTime >= _duration)
            {
                owner.AngularVelocity = Vector3.zero;
                Status = ActionStatus.Success;
            }
        }

        public override void Exit(EnemyActionController owner)
        {
            owner.AngularVelocity = Vector3.zero;
        }
    }
}