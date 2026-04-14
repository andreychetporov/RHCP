using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GroundProbe groundProbe;
    [SerializeField] private HoverSuspension hoverSuspension;
    [SerializeField] private PlayerMovementMotor movementMotor;
    [SerializeField] private JumpMotor jumpMotor;
    [SerializeField] private CursorSlice slice;
    [SerializeField] private Transform model;
    [SerializeField] private Rigidbody rb;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float minRotateVelocity = 0.05f;
    [SerializeField] private Vector3 modelForwardAxis = Vector3.right;

    private bool isSlicing = false;
    private IPlayerInput _input;

    [Inject]
    public void Construct(IPlayerInput input) => _input = input;

    private void Awake()
    {
        if (groundProbe == null)
            groundProbe = GetComponent<GroundProbe>();

        if (hoverSuspension == null)
            hoverSuspension = GetComponent<HoverSuspension>();

        if (movementMotor == null)
            movementMotor = GetComponent<PlayerMovementMotor>();

        if (jumpMotor == null)
            jumpMotor = GetComponent<JumpMotor>();

        if (slice == null)
            slice = GetComponent<CursorSlice>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        float moveX = _input.MoveX;
        bool jumpPressed = _input.JumpPressed;
        bool jumpReleased = _input.JumpReleased;
        bool jumpHeld = _input.JumpHeld;
        bool dashPressed = _input.DashPressed;
        bool crouchHeld = _input.CrouchHeld;

        if (jumpPressed)
            _input.ConsumeJumpPressed();

        if (jumpReleased)
            _input.ConsumeJumpReleased();

        if (dashPressed)
            _input.ConsumeDashPressed();

        groundProbe.TickProbe();
        hoverSuspension.TickSuspension();

        if (dashPressed)
            movementMotor.TryDash(moveX, crouchHeld);

        jumpMotor.TickJump(jumpPressed, jumpReleased, jumpHeld, dt);
        movementMotor.TickMove(moveX, crouchHeld, dt);

        RotateCharacter();
    }

    private void Update()
    {
        if (_input.MousePressed && !isSlicing)
        {
            slice.Reset();
            slice.SetEmitting(true);
            isSlicing = true;
        }

        if (isSlicing)
            slice.UpdateSlice();

        if (!_input.MousePressed && isSlicing)
        {
            slice.SetEmitting(false);
            isSlicing = false;
        }
    }

    private void RotateCharacter()
    {
        if (model == null || rb == null)
            return;

        float velX = rb.linearVelocity.x;

        if (Mathf.Abs(velX) < minRotateVelocity)
            return;

        Vector3 moveDir = velX > 0f ? Vector3.right : Vector3.left;
        Quaternion targetRotation = Quaternion.FromToRotation(modelForwardAxis.normalized, moveDir);

        model.rotation = Quaternion.Slerp(
            model.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }
}