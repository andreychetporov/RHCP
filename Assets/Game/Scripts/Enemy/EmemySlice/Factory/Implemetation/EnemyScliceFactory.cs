using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Enemy.Slice
{
    public class EnemyScliceFactory : IEnemySliceFactory
    {
        [Inject] private EnemySliced _prefab;

        private List<EnemySliced> _pool = new List<EnemySliced>();

        public void SpawnSlicedParts(Transform model, Vector3 velocity, Vector3 cutPoint, Vector3 cutNormal)
        {
            var meshFilter = model.GetComponent<MeshFilter>();
            var meshRenderer = model.GetComponent<MeshRenderer>();
            if (meshFilter == null || meshFilter.sharedMesh == null || meshRenderer == null) return;

            Plane localPlane = MeshSlicer.TransformPlane(new Plane(cutNormal, cutPoint), model);

            MeshSlicer.PartMesh left = MeshSlicer.Slice(meshFilter.sharedMesh, localPlane, true);
            MeshSlicer.PartMesh right = MeshSlicer.Slice(meshFilter.sharedMesh, localPlane, false);

            if (left.IsValid())
                SpawnPart(model, meshRenderer, left, -cutNormal * IEnemySliceFactory.EXPLOED_FORCE + velocity);
            if (right.IsValid())
                SpawnPart(model, meshRenderer, right, cutNormal * IEnemySliceFactory.EXPLOED_FORCE + velocity);
        }

        private void SpawnPart(Transform model, MeshRenderer meshRenderer, MeshSlicer.PartMesh part, Vector3 force)
        {
            EnemySliced sliced = GetPrefab();

            sliced.transform.SetPositionAndRotation(model.position, model.rotation);
            sliced.transform.localScale = model.lossyScale;

            sliced.Activate(part, meshRenderer.materials, force);
        }

        private EnemySliced GetPrefab()
        {
            foreach (EnemySliced prefab in _pool)
            {
                if (prefab != null && !prefab.gameObject.activeSelf)
                {
                    return prefab;
                }
            }

            return GameObject.Instantiate(_prefab);
        }
    }
}