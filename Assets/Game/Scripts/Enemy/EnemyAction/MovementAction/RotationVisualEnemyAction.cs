using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Movement/Rotate")]
    public class RotationVisualEnemyAction : EnemyAction
    {
        [Header("Settings")]
        [SerializeField] private Vector3 _angleDelta = new Vector3(0f, 90f, 0f);
        [SerializeField] private float _duration = 1f;

        [System.NonSerialized] private float _elapsedTime;
        [System.NonSerialized] private Quaternion _startRotation;
        [System.NonSerialized] private Quaternion _targetRotation;

        private Vector3 _axis;
        private float _totalAngle;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            _elapsedTime = 0f;

            _startRotation = owner.VisualModel.transform.rotation;

            // защита от нулевого вектора
            if (_angleDelta == Vector3.zero)
            {
                Status = ActionStatus.Success;
                return;
            }

            _axis = _angleDelta.normalized;
            _totalAngle = _angleDelta.magnitude;

            _targetRotation = _startRotation * Quaternion.AngleAxis(_totalAngle, _axis);

            if (_duration <= 0f)
            {
                owner.VisualModel.transform.rotation = _targetRotation;
                Status = ActionStatus.Success;
            }
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            if (Status != ActionStatus.Running) return;

            _elapsedTime += dt;

            float t = Mathf.Clamp01(_elapsedTime / _duration);
            float currentAngle = _totalAngle * t;

            owner.VisualModel.transform.rotation =
                _startRotation * Quaternion.AngleAxis(currentAngle, _axis);

            if (t >= 1f)
            {
                Status = ActionStatus.Success;
            }
        }

        public override void Exit(BaseEnemyActionController owner)
        {
            // гарантируем точный финальный угол
            owner.VisualModel.transform.rotation = _targetRotation;
        }
    }
}