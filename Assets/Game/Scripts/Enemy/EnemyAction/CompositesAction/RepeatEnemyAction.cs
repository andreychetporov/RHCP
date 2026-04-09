using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    public class RepeatEnemyAction : EnemyAction
    {
        [Header("Settigs")]
        [SerializeField, Tooltip("-1 = inf/loop")] private int _count = -1;

        [Space()]

        [SerializeReference] public EnemyAction Child;

        [System.NonSerialized] private int _currentCount = 0;

        public override void Enter(EnemyActionController owner)
        {
            base.Enter(owner);

            _currentCount = 0;

            Child.Enter(owner);
        }

        public override void Process(EnemyActionController owner, float dt)
        {
            Child.Process(owner, dt);

            if (Child.Status == ActionStatus.Success)
            {
                Child.Exit(owner);
                _currentCount++;

                if (_count != -1 && _currentCount >=  _count)
                {
                    Status = ActionStatus.Success;
                }
                else
                {
                    Child.Enter(owner);
                }
            }
        }
    }
}