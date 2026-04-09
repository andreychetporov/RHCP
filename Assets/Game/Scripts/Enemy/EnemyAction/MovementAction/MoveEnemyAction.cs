using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    public class MoveEnemyAction : EnemyAction
    {
        [Header("Move Settings")]
        [SerializeField] private float _movementSpeed = 5.0f;
        [SerializeField] private Vector3 _moveDirection = Vector3.forward;
        [SerializeField, Tooltip("-1.0f = infinite")]
        private float _maxMoveDistance = -1.0f;

        [System.NonSerialized] private Vector3 _startPosition;
        [System.NonSerialized] private Vector3 _worldDirection;

        public override void Enter(EnemyActionController owner)
        {
            base.Enter(owner);

            _startPosition = owner.transform.position;
            _worldDirection = owner.transform.TransformDirection(_moveDirection.normalized);
        }

        public override void Process(EnemyActionController owner, float dt)
        {
            float traveled = Vector2.Distance(new Vector2(_startPosition.x, _startPosition.z), new Vector2(owner.transform.position.x, owner.transform.position.z));

            if (_maxMoveDistance > 0f && traveled >= _maxMoveDistance)
            {
                owner.Velocity = new Vector3(0, owner.Velocity.y, 0);
                Status = ActionStatus.Success;
                return;
            }

            Vector3 horizontal = _worldDirection * _movementSpeed;
            owner.Velocity = new Vector3(horizontal.x, owner.Velocity.y, horizontal.z);
        }
    }
}