using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Composites")]
    public class DoSecondWhileMainRunningEnemyAction : EnemyAction
    {
        [Header("MAIN")]
        [SerializeReference] private EnemyAction _mainAction;

        [Space()]

        [Header("Second")]
        [SerializeReference] private EnemyAction _secondAction;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            _mainAction.Enter(owner);

            if (_mainAction.Status == ActionStatus.Running)
            {
                _secondAction.Enter(owner);
            }
            else
            {
                Status = _mainAction.Status;
            }
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            _mainAction.Process(owner, dt);

            if (_mainAction.Status == ActionStatus.Running)
            {
                if (_secondAction.Status == ActionStatus.Running)
                {
                    _secondAction.Process(owner, dt);
                }
                else
                {
                    _secondAction.Exit(owner);
                    _secondAction.Enter(owner);
                }
            }
            else
            {
                _secondAction.Exit(owner);
                Status = _mainAction.Status;
            }
        }

        public override void Exit(BaseEnemyActionController owner)
        {
            _mainAction.Exit(owner);
            _secondAction.Exit(owner);
        }
    }
}