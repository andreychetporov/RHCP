using Game.Level;
using UnityEngine;

namespace Game.Enemy.Action
{
    [System.Serializable]
    [EnemyActionCategory("Condition")]
    public class PlayerInZoneConditionEnemyAction : ConditionEnemyAction
    {
        [SerializeField] private Vector3 _boxExtent = Vector3.one;

        public override bool Evaluate(BaseEnemyActionController owner)
        {
            if (LevelBootstrap.Instance == null) { return false; }

            Vector3 local = owner.transform.InverseTransformPoint(LevelBootstrap.Instance.GetPlayerPosition);

            return Mathf.Abs(local.x) <= _boxExtent.x &&
                   Mathf.Abs(local.y) <= _boxExtent.y &&
                   Mathf.Abs(local.z) <= _boxExtent.z;
        }
    }
}