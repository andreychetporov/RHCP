using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Composites")]
    public class ParallelEnemyAction : EnemyAction
    {
        [SerializeReference] public List<EnemyAction> Children;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            foreach (EnemyAction action in Children) { action.Enter(owner); }
        }

        public override void Process(BaseEnemyActionController owner, float dt)
        {
            bool allDone = true;

            foreach (var action in Children)
            {
                if (action.Status == ActionStatus.Running)
                {
                    action.Process(owner, dt);

                    if (action.Status != ActionStatus.Running)
                    {
                        action.Exit(owner);
                    }
                }

                if (action.Status == ActionStatus.Running) { allDone = false; }
            }

            if (allDone) { Status = ActionStatus.Success; }
        }
    }
}