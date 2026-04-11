using Game.Enemy.Action;
using UnityEngine;

namespace Game.Enemy
{
    [RequireComponent(typeof(CharacterController))]
    public class EnemyActionController : BaseEnemyActionController
    {
        [Header("Test")]
        [SerializeField] private EnemyActionBehaviorSO _testSO;

        public override bool IsGrounded => _controller.isGrounded;

        private CharacterController _controller;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public override void Start()
        {
            if (_testSO != null) { _runtimeBehavior = _testSO.Clone(); }

            base.Start();
        }

        private void Update()
        {
            GravityHandle(Time.deltaTime);

            BehaviorHandle(Time.deltaTime);

            if (TargetAngularVelocity.sqrMagnitude > 0.0001f)
            {
                float angleRad = TargetAngularVelocity.magnitude * Time.deltaTime;
                float angleDeg = angleRad * Mathf.Rad2Deg;

                transform.rotation = Quaternion.AngleAxis(angleDeg, TargetAngularVelocity.normalized) * transform.rotation;
            }

            _controller.Move(TargetVelocity * Time.deltaTime);
        }

        public override void SetCollisionData(bool detectCollisions, LayerMask includeLayers, LayerMask excludeLayers)
        {
            _controller.detectCollisions = detectCollisions;
            _controller.includeLayers = includeLayers;
            _controller.excludeLayers = excludeLayers;
        }
    }
}