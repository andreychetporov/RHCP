using System.Linq;
using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    public class CurveMoveEnemyAction : EnemyAction
    {
        [Header("Curves")]
        [SerializeField] private AnimationCurve _xCurve;
        [SerializeField] private AnimationCurve _yCurve;

        [Header("Multipliers")]
        [SerializeField] private float _xMultiplier = 10.0f;
        [SerializeField] private float _yMultiplier = 5.0f;
        [SerializeField] private float _moveSpeed = 1.0f;

        [Header("Rotation")]
        [SerializeField] private bool _lookAtVelocity = false;
        [SerializeField] private float _rotationSpeed = 360.0f;

        [System.NonSerialized] private float _timer;
        [System.NonSerialized] private Vector3 _startPos;

        [System.NonSerialized] private float _xTimeLength;
        [System.NonSerialized] private float _yTimeLength;

        public override void Enter(EnemyActionController owner)
        {
            base.Enter(owner);

            _timer = 0f;
            _startPos = owner.transform.position;
            
            _xTimeLength = _xCurve == null || _xCurve.length == 0 ? 0.0f : _xCurve.keys.Select(k => k.time).Max();
            _yTimeLength = _yCurve == null || _yCurve.length == 0 ? 0.0f : _yCurve.keys.Select(k => k.time).Max();

            owner.Velocity = Vector3.zero;
        }

        public override void Process(EnemyActionController owner, float dt)
        {
            if (dt <= 0.0001f) { return; }

            _timer += dt;

            float tX = Mathf.Clamp01(_timer / _xTimeLength);
            float tY = Mathf.Clamp01(_timer / _yTimeLength);

            float x = _xCurve.Evaluate(tX * _xTimeLength) * _xMultiplier;
            float y = _yCurve.Evaluate(tY * _yTimeLength) * _yMultiplier;

            Vector3 targetPos = _startPos + new Vector3(x, y, 0);

            Vector3 deltaMove = targetPos - owner.transform.position;
            owner.Velocity = deltaMove / dt;

            if (_lookAtVelocity)
            {
                Vector3 moveDir = owner.Velocity.normalized;
                float targetAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;

                float currentAngle = owner.transform.eulerAngles.z;
                float angleDelta = Mathf.DeltaAngle(currentAngle, targetAngle);

                //owner.transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
                owner.AngularVelocity = new Vector3(0f, 0f, Mathf.Sign(angleDelta) * _rotationSpeed);
            }

            if (tX >= 1.0f && tY >= 1.0f)
            {
                Status = ActionStatus.Success;

                owner.Velocity = Vector3.zero;

                if (_lookAtVelocity) { owner.AngularVelocity = Vector3.zero; }
            }
        }

        public override void Exit(EnemyActionController owner)
        {
            owner.Velocity = Vector3.zero;
            if (_lookAtVelocity) { owner.AngularVelocity = Vector3.zero; }
        }
    }
}