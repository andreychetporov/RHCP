using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Composites")]
    public class SequenceEnemyAction : EnemyAction
    {
        [SerializeReference] public List<EnemyAction> Children;

        [System.NonSerialized] private int _currentIndex;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            _currentIndex = 0;

            if (Children != null && Children.Count > 0)
            {
                Children[_currentIndex].Enter(owner);
            }
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            if (_currentIndex >= Children.Count) { return; }

            var current = Children[_currentIndex];

            current.Process(owner, dt);

            if (current.Status == ActionStatus.Running) { return; }
            current.Exit(owner);

            if (current.Status == ActionStatus.Failure)
            {
                Status = ActionStatus.Failure;
                return;
            }

            _currentIndex++;

            if (_currentIndex < Children.Count)
            {
                Children[_currentIndex].Enter(owner);
            }
            else
            {
                Status = ActionStatus.Success;
            }
        }
    }
}