using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Movement/Move")]
    public class MoveEnemyAction : EnemyAction
    {
        [Header("Move Settings")]
        [SerializeField] private float _movementSpeed = 5.0f;
        [SerializeField] private Vector3 _moveDirection = Vector3.forward;
        [SerializeField, Tooltip("-1.0f = infinite")]
        private float _maxMoveDistance = -1.0f;

        [System.NonSerialized] private Vector3 _startPosition;
        [System.NonSerialized] private Vector3 _worldDirection;
        [System.NonSerialized] private Vector3? _endPosition;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            _endPosition = null;

            _startPosition = owner.transform.position;
            _worldDirection = owner.transform.TransformDirection(_moveDirection.normalized);

            if (_maxMoveDistance < 0.0f)
            {
                _endPosition = _startPosition + _worldDirection * _movementSpeed;
            }
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            float traveled = Vector2.Distance(new Vector2(_startPosition.x, _startPosition.z), new Vector2(owner.transform.position.x, owner.transform.position.z));

            if (_maxMoveDistance > 0f && traveled >= _maxMoveDistance)
            {
                owner.TargetVelocity = new Vector3(0, owner.TargetVelocity.y, 0);
                Status = ActionStatus.Success;
                return;
            }

            Vector3 horizontal = _worldDirection * _movementSpeed;
            owner.TargetVelocity = new Vector3(horizontal.x, owner.TargetVelocity.y, horizontal.z);
        }

        public override void Exit(BaseEnemyActionController owner)
        {
            if (_endPosition.HasValue)
            {
                owner.transform.position = _endPosition.Value;
            }
        }
    }
}