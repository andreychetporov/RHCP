using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GroundProbe groundProbe;
    [SerializeField] private HoverSuspension hoverSuspension;
    [SerializeField] private PlayerMovementMotor movementMotor;
    [SerializeField] private JumpMotor jumpMotor;

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
    }
}