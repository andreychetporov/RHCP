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
        if (groundProbe == null) groundProbe = GetComponent<GroundProbe>();
        if (hoverSuspension == null) hoverSuspension = GetComponent<HoverSuspension>();
        if (movementMotor == null) movementMotor = GetComponent<PlayerMovementMotor>();
        if (jumpMotor == null) jumpMotor = GetComponent<JumpMotor>();
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        bool jumpPressed = _input.JumpPressed;
        bool jumpReleased = _input.JumpReleased;
        bool jumpHeld = _input.JumpHeld;

        if (jumpPressed) _input.ConsumeJumpPressed();
        if (jumpReleased) _input.ConsumeJumpReleased();

        groundProbe.TickProbe();
        hoverSuspension.TickSuspension();
        jumpMotor.TickJump(jumpPressed, jumpReleased, jumpHeld, dt);
        movementMotor.TickMove(_input.MoveX, dt);
    }
}