using Game.Enemy.Action;
using UnityEngine;

namespace Game.Enemy
{
    [RequireComponent(typeof(CharacterController))]
    public class EnemyCharacterController : BaseEnemyActionController
    {
        [Header("Reference")]
        [SerializeField] private Transform _model;
        [SerializeField] private GroundProbe _groundProbe;

        [Header("GroundCheck")]
        [SerializeField] private float _groundDistance = 1.0f;

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

        [Header("Hit Slow")]
        [SerializeField] private bool _hitSlowEnabled = true;
        [SerializeField] private float _hitSlowMultiplier = 0.35f;
        [SerializeField] private float _hitSlowDuration = 0.2f;
        [SerializeField] private float _hitSlowDownSpeed = 18f;
        [SerializeField] private float _hitRecoverSpeed = 8f;

        private CharacterController _controller;

        private bool _wasGrounded;
        private float _springValue;
        private float _springVelocity;
        private float _lastVerticalVelocity;

        private Vector3 _baseScale;

        private float _hitSlowTimer;
        private float _currentMoveSpeedMultiplier = 1f;

        public override bool IsGrounded => SuspensionEnabled
            ? _groundProbe.GroundDistance <= _groundDistance
            : _groundProbe.IsGrounded;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _baseScale = _model.localScale;
        }

        public override void SetCollisionData(bool detectCollisions, LayerMask includeLayers, LayerMask excludeLayers)
        {
            _controller.detectCollisions = detectCollisions;
            _controller.includeLayers = includeLayers;
            _controller.excludeLayers = excludeLayers;
        }

        public override void ApplyHitSlow()
        {
            if (!_hitSlowEnabled) { return; }

            _hitSlowTimer = _hitSlowDuration;
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _lastVerticalVelocity = TargetVelocity.y;

            GravityHandle(dt);
            BehaviorHandle(dt);
            UpdateHitSlow(dt);

            Vector3 finalVelocity = GetSlowedVelocity(TargetVelocity);

            if (TargetAngularVelocity.sqrMagnitude > 0.0001f)
            {
                Vector3 slowedAngularVelocity = TargetAngularVelocity * _currentMoveSpeedMultiplier;

                float angleRad = slowedAngularVelocity.magnitude * dt;
                float angleDeg = angleRad * Mathf.Rad2Deg;

                if (angleDeg > 0.0001f)
                {
                    transform.rotation =
                        Quaternion.AngleAxis(angleDeg, slowedAngularVelocity.normalized) * transform.rotation;
                }
            }

            _controller.Move(finalVelocity * dt);

            SquashUpdate();
            _groundProbe.TickProbe();

            if (SuspensionEnabled)
            {
                TickSuspension(dt);
            }
        }

        private void UpdateHitSlow(float dt)
        {
            float targetMultiplier = 1f;
            float changeSpeed = _hitRecoverSpeed;

            if (_hitSlowTimer > 0f)
            {
                _hitSlowTimer -= dt;
                targetMultiplier = _hitSlowMultiplier;
                changeSpeed = _hitSlowDownSpeed;
            }

            _currentMoveSpeedMultiplier = Mathf.MoveTowards(
                _currentMoveSpeedMultiplier,
                targetMultiplier,
                changeSpeed * dt
            );
        }

        private Vector3 GetSlowedVelocity(Vector3 velocity)
        {
            Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z) * _currentMoveSpeedMultiplier;
            return new Vector3(horizontal.x, velocity.y, horizontal.z);
        }

        private void SquashUpdate()
        {
            if (!SquashEnabled) { return; }

            bool grounded = IsGrounded;

            if (grounded && !_wasGrounded) { OnLand(); }

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
            if (!SuspensionEnabled) { return;}

            if (_groundProbe == null || !_groundProbe.HasHit) { return;}

            if (!IsGrounded || IsJumping) { return; }

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

            if (Mathf.Abs(Gravity) <= 0.001f) { return; }

            if (!IsGrounded)
            {
                TargetVelocity.y += Gravity * dt;
            }
        }
    }
}