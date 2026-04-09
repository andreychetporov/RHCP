using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    public class SetCollisionLayerEnemyAction :EnemyAction
    {
        [Header("Settings")]
        [SerializeField] private bool _detectCollisions = true;
        [SerializeField] private LayerMask _includeLayers = -1;
        [SerializeField] private LayerMask _excludeLayers = -1;

        public override void Enter(EnemyActionController owner)
        {
            base.Enter(owner);

            owner.Controller.detectCollisions = _detectCollisions;
            owner.Controller.includeLayers = _includeLayers;
            owner.Controller.excludeLayers = _excludeLayers;

            Status = ActionStatus.Success;
        }
    }
}