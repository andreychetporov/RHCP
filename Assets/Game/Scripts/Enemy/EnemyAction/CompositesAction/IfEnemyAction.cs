using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Composites")]
    public class IfEnemyAction : EnemyAction
    {
        [Header("Rule")]
        [SerializeReference] private ConditionEnemyAction _condition;

        [Header("Action")]
        [SerializeReference] private EnemyAction _trueAction;
        [SerializeReference] private EnemyAction _falseAction;

        [System.NonSerialized] private EnemyAction _currentAction;
        [System.NonSerialized] private bool _initialized;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            _initialized = false;
            _currentAction = null;
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            bool result = _condition.Evaluate(owner);

            if (!_initialized)
            {
                Switch(owner, result);
                _initialized = true;
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

        public override void Exit(BaseEnemyActionController owner)
        {
            _currentAction?.Exit(owner);
            _currentAction = null;
        }

        private void Switch(BaseEnemyActionController owner, bool result)
        {
            _currentAction?.Exit(owner);

            _currentAction = result ? _trueAction : _falseAction;

            if (_currentAction != null)
            {
                _currentAction.Status = ActionStatus.Running;
                _currentAction.Enter(owner);
            }
        }
    }
}