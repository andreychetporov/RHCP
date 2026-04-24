using System.Collections;
using UnityEngine;

namespace Game.Enemy.Slice
{
    [RequireComponent(typeof(Rigidbody), typeof(MeshRenderer), typeof(MeshFilter))]
    public class EnemySliced : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _lifeTime = 5.0f;

        private Rigidbody _rb;
        private MeshFilter _mf;
        private MeshRenderer _mr;

        private Coroutine _lifeTimeCor;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _mf = GetComponent<MeshFilter>();
            _mr = GetComponent<MeshRenderer>();
        }

        public void Activate(MeshSlicer.PartMesh part, Material[] materials, Vector3 force)
        {
            if (_lifeTimeCor != null) { StopCoroutine(_lifeTimeCor); }

            {
                _mf.mesh = part.Mesh;
                _mr.materials = materials;
            }

            {
                _rb.AddForce(force, ForceMode.Impulse);
                _rb.AddTorque(Random.onUnitSphere * 3f, ForceMode.Impulse);
            }

            gameObject.SetActive(true);
            _lifeTimeCor = StartCoroutine(LifeCur());
        }

        private IEnumerator LifeCur()
        {
            yield return new WaitForSeconds(_lifeTime);

            gameObject.SetActive(false);
        }
    }
}