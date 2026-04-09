using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    public class IfPlayerInZone : EnemyAction
    {
        [Header("Settings")]
        [SerializeField] private Vector3 _boxExtent = Vector3.one;
        [SerializeField] private bool _canTranslateBack = true;

        [SerializeReference] private EnemyAction _falseAction;
        [SerializeReference] private EnemyAction _trueAction;

        [System.NonSerialized] private EnemyAction _currentAction;
        [System.NonSerialized] private bool _hasEvaluated;
        [System.NonSerialized] private bool _lastConditionResult;

        public override void Enter(EnemyActionController owner)
        {
            base.Enter(owner);

            _hasEvaluated = false;
            _currentAction = null;
        }

        public override void Process(EnemyActionController owner, float dt)
        {
            bool isPlayerInZone = CheckIfPlayerInZone(owner);

            if (!_hasEvaluated || (_canTranslateBack && isPlayerInZone != _lastConditionResult))
            {
                SwitchAction(owner, isPlayerInZone);

                _hasEvaluated = true;
                _lastConditionResult = isPlayerInZone;
            }

            if (_currentAction != null)
            {
                _currentAction.Process(owner, dt);

                if (_currentAction.Status != ActionStatus.Running)
                {
                    Status = _currentAction.Status;
                }
            }
            else
            {
                Status = ActionStatus.Success;
            }
        }

        public override void Exit(EnemyActionController owner)
        {
            base.Exit(owner);

            if (_currentAction != null)
            {
                _currentAction.Exit(owner);

                _currentAction = null;
            }
        }

        private bool CheckIfPlayerInZone(EnemyActionController owner)
        {
            if (owner.Target == null) { return false; }

            Vector3 localTargetPos = owner.transform.InverseTransformPoint(owner.Target.position);

            return Mathf.Abs(localTargetPos.x) <= _boxExtent.x && Mathf.Abs(localTargetPos.y) <= _boxExtent.y && Mathf.Abs(localTargetPos.z) <= _boxExtent.z;
        }

        private void SwitchAction(EnemyActionController owner, bool isTrue)
        {
            if (_currentAction != null)
            {
                _currentAction.Exit(owner);
            }

            _currentAction = isTrue ? _trueAction : _falseAction;

            if (_currentAction != null)
            {
                _currentAction.Status = ActionStatus.Running;
                _currentAction.Enter(owner);
            }
        }
    }
}