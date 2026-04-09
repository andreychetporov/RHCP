using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    public class SequenceEnemyAction : EnemyAction
    {
        [SerializeReference] public List<EnemyAction> Children;

        [System.NonSerialized] private int _currentIndex;

        public override void Enter(EnemyActionController owner)
        {
            base.Enter(owner);

            _currentIndex = 0;

            if (Children != null && Children.Count > 0)
            {
                Children[_currentIndex].Enter(owner);
            }
        }

        public override void Process(EnemyActionController owner, float dt)
        {
            if (_currentIndex >= Children.Count) { return; }

            Children[_currentIndex].Process(owner, dt);

            if (Children[_currentIndex].Status == ActionStatus.Success)
            {
                Children[_currentIndex].Exit(owner);

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
}