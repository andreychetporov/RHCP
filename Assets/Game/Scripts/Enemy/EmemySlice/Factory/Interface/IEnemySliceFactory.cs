using UnityEngine;

namespace Game.Enemy.Slice
{
    public interface IEnemySliceFactory
    {
        void SpawnSlicedParts(Transform model, Color mainColor, Vector3 velocity, Vector3 cutPoint, Vector3 cutNormal);
    }
}