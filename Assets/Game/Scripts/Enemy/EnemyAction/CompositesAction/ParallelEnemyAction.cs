using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    public class ParallelEnemyAction : EnemyAction
    {
        [SerializeReference] public List<EnemyAction> Children;

        public override void Enter(EnemyActionController owner)
        {
            base.Enter(owner);

            foreach (EnemyAction action in Children) { action.Enter(owner); }
        }

        public override void Process(EnemyActionController owner, float dt)
        {
            bool allDone = true;

            foreach (EnemyAction action in Children)
            {
                if (action.Status == ActionStatus.Running)
                {
                    action.Process(owner, dt);
                    allDone = false;
                }
            }

            if (allDone) { Status = ActionStatus.Success; }
        }
    }
}