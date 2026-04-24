using UnityEngine;

namespace Game.Enemy.Slice
{
    public interface IEnemySliceFactory
    {
        public const float EXPLOED_FORCE = 6.0f;

        void SpawnSlicedParts(Transform model, Vector3 velocity, Vector3 cutPoint, Vector3 cutNormal);
    }
}