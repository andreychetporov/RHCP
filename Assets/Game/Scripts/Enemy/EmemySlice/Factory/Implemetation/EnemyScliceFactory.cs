using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Enemy.Slice
{
    public class EnemyScliceFactory : IEnemySliceFactory
    {
        private EnemySliced _prefab;

        private List<EnemySliced> _pool = new List<EnemySliced>();

        [Inject]
        private void Initialize(EnemySliced prefab)
        {
            _prefab = prefab;

            for (int i = 0; i < 20;  i++)
            {
                var go = GameObject.Instantiate(_prefab);
                go.gameObject.SetActive(false);
                _pool.Add(go);
            }
        }

        public void SpawnSlicedParts(Transform model, Color mainColor, Vector3 velocity, Vector3 cutPoint, Vector3 cutNormal)
        {
            var meshFilter = model.GetComponent<MeshFilter>();
            var meshRenderer = model.GetComponent<MeshRenderer>();
            if (meshFilter == null || meshFilter.sharedMesh == null || meshRenderer == null) return;

            Plane localPlane = MeshSlicer.TransformPlane(new Plane(cutNormal, cutPoint), model);

            MeshSlicer.PartMesh left = MeshSlicer.Slice(meshFilter.sharedMesh, localPlane, true);
            MeshSlicer.PartMesh right = MeshSlicer.Slice(meshFilter.sharedMesh, localPlane, false);

            if (left.IsValid())
                SpawnPart(model, meshRenderer, left, mainColor, -cutNormal, velocity);
            if (right.IsValid())                                          
                SpawnPart(model, meshRenderer, right, mainColor, cutNormal, velocity);
        }

        private void SpawnPart(Transform model, MeshRenderer meshRenderer, MeshSlicer.PartMesh part, Color mainColor, Vector3 cutNormal, Vector3 baseVelocity)
        {
            EnemySliced sliced = GetPrefab();
            sliced.transform.SetPositionAndRotation(model.position, model.rotation);
            sliced.transform.localScale = model.localScale;

            sliced.Activate(part, meshRenderer.sharedMaterials, mainColor, cutNormal, baseVelocity);
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