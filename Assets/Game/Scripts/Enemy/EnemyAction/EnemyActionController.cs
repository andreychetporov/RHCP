using Game.Enemy.Action;
using UnityEngine;

namespace Game.Enemy
{
    [RequireComponent(typeof(CharacterController))]
    public class EnemyActionController : MonoBehaviour
    {
        [Header("Test")]
        [SerializeField] private EnemyActionBehaviorSO _testSO;
        [SerializeField] private Transform _target;

        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        public float Gravity = -20.0f;

        public CharacterController Controller { get; private set; }
        public Transform Target => _target;
        public bool IsGrounded => Controller.isGrounded;

        private EnemyActionBehaviorSO _runtimeBehavior;

        private void Awake()
        {
            Controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            if (_testSO != null)
            {
                _runtimeBehavior = _testSO.Clone();
                _runtimeBehavior.RootAction.Enter(this);
            }
        }

        private void Update()
        {
            if (Gravity <= 0.0f)
            {
                if (!Controller.isGrounded)
                {
                    Velocity.y += Gravity * Time.deltaTime;
                }
                else if (Velocity.y < 0)
                {
                    Velocity.y = -2f;
                }
            }

            if (_runtimeBehavior != null)
            {
                _runtimeBehavior.RootAction.Process(this, Time.deltaTime);
            }

            if (AngularVelocity != Vector3.zero)
            {
                transform.Rotate(AngularVelocity * Time.deltaTime, Space.Self);
            }

            Controller.Move(Velocity * Time.deltaTime);
        }
    }
}