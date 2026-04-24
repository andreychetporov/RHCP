using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy.Slice
{
    public static class MeshSlicer
    {
        public static Plane TransformPlane(Plane worldPlane, Transform transform)
        {
            Vector3 localNormal = transform.InverseTransformDirection(worldPlane.normal);
            Vector3 worldPoint = worldPlane.normal * (-worldPlane.distance);
            Vector3 localPoint = transform.InverseTransformPoint(worldPoint);
            return new Plane(localNormal.normalized, localPoint);
        }

        public static PartMesh Slice(Mesh source, Plane plane, bool keepLeft)
        {
            var result = new PartMesh();
            var ray1 = new Ray();
            var ray2 = new Ray();

            var capEdges = new List<(Vector3 a, Vector3 b, Vector2 uvA, Vector2 uvB)>();

            for (int sub = 0; sub < source.subMeshCount; sub++)
            {
                int[] triangles = source.GetTriangles(sub);

                for (int j = 0; j < triangles.Length; j += 3)
                {
                    Vector3 v0 = source.vertices[triangles[j]];
                    Vector3 v1 = source.vertices[triangles[j + 1]];
                    Vector3 v2 = source.vertices[triangles[j + 2]];

                    bool sideA = plane.GetSide(v0) == keepLeft;
                    bool sideB = plane.GetSide(v1) == keepLeft;
                    bool sideC = plane.GetSide(v2) == keepLeft;
                    int sideCount = (sideA ? 1 : 0) + (sideB ? 1 : 0) + (sideC ? 1 : 0);

                    if (sideCount == 0) continue;
                    if (sideCount == 3)
                    {
                        result.AddTriangle(sub,
                            v0, v1, v2,
                            source.normals[triangles[j]],
                            source.normals[triangles[j + 1]],
                            source.normals[triangles[j + 2]],
                            source.uv[triangles[j]],
                            source.uv[triangles[j + 1]],
                            source.uv[triangles[j + 2]]);
                        continue;
                    }

                    int single = sideB == sideC ? 0 : sideA == sideC ? 1 : 2;
                    var verts = new[] { v0, v1, v2 };

                    ray1.origin = verts[single];
                    Vector3 dir1 = verts[(single + 1) % 3] - ray1.origin;
                    ray1.direction = dir1;
                    plane.Raycast(ray1, out float enter1);
                    float lerp1 = enter1 / dir1.magnitude;

                    ray2.origin = verts[single];
                    Vector3 dir2 = verts[(single + 2) % 3] - ray2.origin;
                    ray2.direction = dir2;
                    plane.Raycast(ray2, out float enter2);
                    float lerp2 = enter2 / dir2.magnitude;

                    Vector3 cut1 = ray1.origin + dir1.normalized * enter1;
                    Vector3 cut2 = ray2.origin + dir2.normalized * enter2;

                    Vector2 uv1 = Vector2.Lerp(
                        source.uv[triangles[j + single]],
                        source.uv[triangles[j + (single + 1) % 3]], lerp1);
                    Vector2 uv2 = Vector2.Lerp(
                        source.uv[triangles[j + single]],
                        source.uv[triangles[j + (single + 2) % 3]], lerp2);

                    Vector3 nSingle = source.normals[triangles[j + single]];
                    Vector3 n1 = source.normals[triangles[j + (single + 1) % 3]];
                    Vector3 n2 = source.normals[triangles[j + (single + 2) % 3]];

                    capEdges.Add((cut1, cut2, uv1, uv2));

                    if (sideCount == 1)
                    {
                        result.AddTriangle(sub,
                            verts[single], cut1, cut2,
                            nSingle,
                            Vector3.Lerp(nSingle, n1, lerp1),
                            Vector3.Lerp(nSingle, n2, lerp2),
                            source.uv[triangles[j + single]], uv1, uv2);
                    }
                    else
                    {
                        result.AddTriangle(sub,
                            cut1, verts[(single + 1) % 3], verts[(single + 2) % 3],
                            Vector3.Lerp(nSingle, n1, lerp1), n1, n2,
                            uv1, source.uv[triangles[j + (single + 1) % 3]],
                            source.uv[triangles[j + (single + 2) % 3]]);
                        result.AddTriangle(sub,
                            cut1, verts[(single + 2) % 3], cut2,
                            Vector3.Lerp(nSingle, n1, lerp1), n2,
                            Vector3.Lerp(nSingle, n2, lerp2),
                            uv1, source.uv[triangles[j + (single + 2) % 3]], uv2);
                    }
                }
            }

            BuildCap(result, capEdges, plane.normal, keepLeft);

            result.BuildMesh();
            return result;
        }

        private static void BuildCap(PartMesh result, List<(Vector3 a, Vector3 b, Vector2 uvA, Vector2 uvB)> edges,
            Vector3 planeNormal, bool keepLeft)
        {
            if (edges.Count == 0) return;

            var centroid = Vector3.zero;
            var centroidUV = Vector2.zero;
            foreach (var (a, b, uvA, uvB) in edges)
            {
                centroid += a + b;
                centroidUV += uvA + uvB;
            }
            centroid /= edges.Count * 2f;
            centroidUV /= edges.Count * 2f;

            Vector3 capNormal = keepLeft ? -planeNormal : planeNormal;

            foreach (var (a, b, uvA, uvB) in edges)
            {
                Vector3 p0 = centroid, p1 = a, p2 = b;
                Vector2 uv0 = centroidUV, uv1 = uvA, uv2 = uvB;

                Vector3 triNormal = Vector3.Cross(p1 - p0, p2 - p0).normalized;
                if (Vector3.Dot(triNormal, capNormal) < 0f)
                {
                    (p1, p2) = (p2, p1);
                    (uv1, uv2) = (uv2, uv1);
                }

                result.AddTriangle(0,
                    p0, p1, p2,
                    capNormal, capNormal, capNormal,
                    uv0, uv1, uv2);
            }
        }

        public class PartMesh
        {
            private List<Vector3> _v = new();
            private List<Vector3> _n = new();
            private List<List<int>> _t = new();
            private List<Vector2> _uv = new();
            private Bounds _bounds = new();

            public Mesh Mesh { get; private set; }
            public Bounds Bounds => _bounds;
            public bool IsValid() => _v.Count > 0;

            public void AddTriangle(int sub,
                Vector3 v0, Vector3 v1, Vector3 v2,
                Vector3 n0, Vector3 n1, Vector3 n2,
                Vector2 uv0, Vector2 uv1, Vector2 uv2)
            {
                while (_t.Count <= sub) _t.Add(new List<int>());
                int baseIdx = _v.Count;

                _t[sub].Add(baseIdx);
                _t[sub].Add(baseIdx + 1);
                _t[sub].Add(baseIdx + 2);

                _v.Add(v0); _v.Add(v1); _v.Add(v2);
                _n.Add(n0); _n.Add(n1); _n.Add(n2);
                _uv.Add(uv0); _uv.Add(uv1); _uv.Add(uv2);

                _bounds.Encapsulate(v0);
                _bounds.Encapsulate(v1);
                _bounds.Encapsulate(v2);
            }

            public void BuildMesh()
            {
                Mesh = new Mesh();
                Mesh.vertices = _v.ToArray();
                Mesh.normals = _n.ToArray();
                Mesh.uv = _uv.ToArray();
                Mesh.subMeshCount = _t.Count;
                for (int i = 0; i < _t.Count; i++)
                    Mesh.SetTriangles(_t[i], i);
                Mesh.RecalculateBounds();
                _bounds = Mesh.bounds;
            }
        }
    }
}