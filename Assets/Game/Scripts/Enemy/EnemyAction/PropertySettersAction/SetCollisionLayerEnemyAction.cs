using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("PropertySetters")]
    public class SetCollisionLayerEnemyAction :EnemyAction
    {
        [Header("Settings")]
        [SerializeField] private bool _detectCollisions = true;
        [SerializeField] private LayerMask _includeLayers = -1;
        [SerializeField] private LayerMask _excludeLayers = -1;

        public override void Enter(BaseEnemyActionController owner)
        {
            base.Enter(owner);

            owner.SetCollisionData(_detectCollisions, _includeLayers, _excludeLayers);

            Status = ActionStatus.Success;
        }
    }
}