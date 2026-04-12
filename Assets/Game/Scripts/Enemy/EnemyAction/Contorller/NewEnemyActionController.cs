using Game.Enemy.Action;
using UnityEngine;

namespace Game.Enemy
{
    [RequireComponent(typeof(CharacterController))]
    public class NewEnemyCharacterController : BaseEnemyActionController
    {
        [Header("Test")]
        [SerializeField] private EnemyActionBehaviorSO _testSO;

        [SerializeField] private Transform _model;

        [SerializeField] private GroundProbe _groundProbe;

        [Header("Squash")]
        [SerializeField] private bool SquashEnabled = true;
        [SerializeField] private float squashOnJump = 0.2f;
        [SerializeField] private float squashOnLandMin = 0.1f;
        [SerializeField] private float squashOnLandMax = 0.5f;
        [SerializeField] private float landSpeedMin = 1f;
        [SerializeField] private float landSpeedMax = 15f;
        [SerializeField] private float springStiffness = 280f;
        [SerializeField] private float springDamping = 18f;

        [Header("Suspension")]
        [SerializeField] private bool SuspensionEnabled = true;
        [SerializeField] private float rideHeight = 1.0f;
        [SerializeField] private float springStrength = 120f;
        [SerializeField] private float springDamper = 25f;
        [SerializeField] private float maxSpringForce = 200f;

        [SerializeField] private float _groundDistance = 1.0f;


        private CharacterController _controller;

        private bool _wasGrounded;
        private float _springValue;
        private float _springVelocity;
        private float _lastVerticalVelocity;

        private Vector3 _baseScale;

        public override bool IsGrounded => SuspensionEnabled ? _groundProbe.GroundDistance <= _groundDistance : _groundProbe.IsGrounded;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _baseScale = _model.localScale;
        }

        public override void Start()
        {
            if (_testSO != null)
            {
                _runtimeBehavior = _testSO.Clone();
            }

            base.Start();
        }

        private void FixedUpdate()
        {
            Debug.Log(IsGrounded);
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _lastVerticalVelocity = TargetVelocity.y;

            GravityHandle(dt);
            BehaviorHandle(dt);

            if (TargetAngularVelocity.sqrMagnitude > 0.0001f)
            {
                float angleRad = TargetAngularVelocity.magnitude * dt;
                float angleDeg = angleRad * Mathf.Rad2Deg;

                transform.rotation =
                    Quaternion.AngleAxis(angleDeg, TargetAngularVelocity.normalized) * transform.rotation;
            }

            _controller.Move(TargetVelocity * dt);
            SquashUpdate();
            _groundProbe.TickProbe();

            if (SuspensionEnabled) { TickSuspension(dt); }
        }

        public override void SetCollisionData(bool detectCollisions, LayerMask includeLayers, LayerMask excludeLayers)
        {
            _controller.detectCollisions = detectCollisions;
            _controller.includeLayers = includeLayers;
            _controller.excludeLayers = excludeLayers;
        }

        private void SquashUpdate()
        {
            if (!SquashEnabled) { return; }
            
            bool grounded = IsGrounded;

            if (grounded && !_wasGrounded)
                OnLand();

            _wasGrounded = grounded;

            float springForce = -springStiffness * _springValue;
            float dampForce = -springDamping * _springVelocity;

            _springVelocity += (springForce + dampForce) * Time.deltaTime;
            _springValue += _springVelocity * Time.deltaTime;

            float scaleY = _baseScale.y * (1f + _springValue);
            float scaleX = _baseScale.x * (1f - _springValue * 0.5f);

            _model.localScale = new Vector3(scaleX, scaleY, scaleX);
        }

        private void OnLand()
        {
            float impactSpeed = Mathf.Abs(_lastVerticalVelocity);

            float t = Mathf.InverseLerp(landSpeedMin, landSpeedMax, impactSpeed);
            float squash = Mathf.Lerp(squashOnLandMin, squashOnLandMax, t);

            _springVelocity -= squash * springStiffness;
        }

        public void TickSuspension(float dt)
        {
            if (!SuspensionEnabled)
                return;

            if (_groundProbe == null || !_groundProbe.HasHit)
                return;

            if (!IsGrounded)
                return;

            if (IsJumping)
                return;

            Rigidbody hitBody = _groundProbe.GroundRigidbody;

            Vector3 rayDir = Vector3.down;

            Vector3 hitBodyVelocity = hitBody != null ? hitBody.linearVelocity : Vector3.zero;

            float bodyVelAlongRay = Vector3.Dot(rayDir, TargetVelocity);

            float hitVelAlongRay = Vector3.Dot(rayDir, hitBodyVelocity);
            float relativeVel = bodyVelAlongRay - hitVelAlongRay;

            float currentHeight = _groundProbe.GroundDistance;
            float heightError = currentHeight - rideHeight;

            float springAccel = (heightError * springStrength) - (relativeVel * springDamper);
            springAccel = Mathf.Clamp(springAccel, -maxSpringForce, maxSpringForce);

            TargetVelocity += rayDir * springAccel * dt;
        }

        public override void GravityHandle(float dt)
        {
            if (IsJumping && IsGrounded)
            {
                IsJumping = false;
            }

            if (Mathf.Abs(Gravity) <= 0.001f)
                return;

            if (!IsGrounded)
            {
                TargetVelocity.y += Gravity * dt;
            }
        }
    }
}