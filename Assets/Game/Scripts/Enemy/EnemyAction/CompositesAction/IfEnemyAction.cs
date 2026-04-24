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

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);
            _currentAction = null;
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            bool result = _condition.Evaluate(owner);

            EnemyAction desiredAction = result ? _trueAction : _falseAction;

            if (_currentAction != desiredAction)
            {
                _currentAction?.Exit(owner);

                _currentAction = desiredAction;

                if (_currentAction != null)
                {
                    _currentAction.Status = ActionStatus.Running;
                    _currentAction.Enter(owner);
                }
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
    }
}