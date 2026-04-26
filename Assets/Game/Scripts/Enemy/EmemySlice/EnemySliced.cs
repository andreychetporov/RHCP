using Game.Blood;
using System.Collections;
using UnityEngine;

namespace Game.Enemy.Slice
{
    [RequireComponent(typeof(Rigidbody), typeof(MeshRenderer), typeof(MeshFilter))]
    [RequireComponent(typeof(MeshCollider))]
    public class EnemySliced : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _lifeTime = 5.0f;
        [SerializeField] private float _expoledForce = 5.0f;

        private Rigidbody _rb;
        private MeshFilter _mf;
        private MeshRenderer _mr;
        private MeshCollider _mc;

        private Coroutine _lifeTimeCor;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _mf = GetComponent<MeshFilter>();
            _mr = GetComponent<MeshRenderer>();
            _mc = GetComponent<MeshCollider>();
        }

        public void Activate(MeshSlicer.PartMesh part, Material[] materials, Color mainColor, Vector3 cutNormal, Vector3 baseVelocity)
        {
            if (_lifeTimeCor != null) { StopCoroutine(_lifeTimeCor); }

            {
                _mf.mesh = part.Mesh;
                _mr.materials = materials;
                _mc.sharedMesh = part.Mesh;
            }

            gameObject.SetActive(true);

            {
                _rb.linearVelocity = Vector3.zero;
                _rb.AddForce(cutNormal * _expoledForce + baseVelocity, ForceMode.Impulse);

                _rb.angularVelocity = Vector3.zero;
                _rb.AddTorque(Random.onUnitSphere * 0.1f, ForceMode.Impulse);
            }

            {
                BloodManager.Instance.GetForDeath().Initialize(transform.position, mainColor).Play();
            }

            _lifeTimeCor = StartCoroutine(LifeCur());
        }

        private IEnumerator LifeCur()
        {
            yield return new WaitForSeconds(_lifeTime);

            gameObject.SetActive(false);
        }
    }
}