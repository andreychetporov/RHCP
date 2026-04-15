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
            DrawBox(owner.transform);

            if (LevelBootstrap.Instance == null) { return false; }

            Vector3 local = owner.transform.InverseTransformPoint(LevelBootstrap.Instance.GetPlayerPosition);

            return Mathf.Abs(local.x) <= _boxExtent.x &&
                   Mathf.Abs(local.y) <= _boxExtent.y &&
                   Mathf.Abs(local.z) <= _boxExtent.z;
        }

        private void DrawBox(Transform transform)
        {
            Vector3 c = transform.position;

            // локальные углы бокса
            Vector3[] localCorners = new Vector3[8]
            {
        new Vector3(-_boxExtent.x, -_boxExtent.y, -_boxExtent.z),
        new Vector3(_boxExtent.x, -_boxExtent.y, -_boxExtent.z),
        new Vector3(_boxExtent.x, -_boxExtent.y, _boxExtent.z),
        new Vector3(-_boxExtent.x, -_boxExtent.y, _boxExtent.z),

        new Vector3(-_boxExtent.x, _boxExtent.y, -_boxExtent.z),
        new Vector3(_boxExtent.x, _boxExtent.y, -_boxExtent.z),
        new Vector3(_boxExtent.x, _boxExtent.y, _boxExtent.z),
        new Vector3(-_boxExtent.x, _boxExtent.y, _boxExtent.z)
            };

            // переводим в world space
            Vector3[] worldCorners = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                worldCorners[i] = transform.TransformPoint(localCorners[i]);
            }

            // низ
            Debug.DrawLine(worldCorners[0], worldCorners[1], Color.red);
            Debug.DrawLine(worldCorners[1], worldCorners[2], Color.red);
            Debug.DrawLine(worldCorners[2], worldCorners[3], Color.red);
            Debug.DrawLine(worldCorners[3], worldCorners[0], Color.red);

            // верх
            Debug.DrawLine(worldCorners[4], worldCorners[5], Color.green);
            Debug.DrawLine(worldCorners[5], worldCorners[6], Color.green);
            Debug.DrawLine(worldCorners[6], worldCorners[7], Color.green);
            Debug.DrawLine(worldCorners[7], worldCorners[4], Color.green);

            // вертикали
            Debug.DrawLine(worldCorners[0], worldCorners[4], Color.blue);
            Debug.DrawLine(worldCorners[1], worldCorners[5], Color.blue);
            Debug.DrawLine(worldCorners[2], worldCorners[6], Color.blue);
            Debug.DrawLine(worldCorners[3], worldCorners[7], Color.blue);
        }
    }
}