using UnityEngine.InputSystem;

public class PlayerInputReader : IPlayerInput
{
    private readonly PlayerInputActions _actions;

    private bool _jumpPressed;
    private bool _jumpReleased;
    private bool _dashPressed;

    public float MoveX => _actions.Player.Move.ReadValue<float>();

    public bool JumpHeld => _actions.Player.Jump.IsPressed();
    public bool CrouchHeld => _actions.Player.Crouch.IsPressed();

    public bool JumpPressed => _jumpPressed;
    public bool JumpReleased => _jumpReleased;
    public bool DashPressed => _dashPressed;

    public void ConsumeJumpPressed() => _jumpPressed = false;
    public void ConsumeJumpReleased() => _jumpReleased = false;
    public void ConsumeDashPressed() => _dashPressed = false;

    public PlayerInputReader()
    {
        _actions = new PlayerInputActions();

        _actions.Player.Jump.performed += ctx => _jumpPressed = true;
        _actions.Player.Jump.canceled += ctx => _jumpReleased = true;

        _actions.Player.Dash.performed += ctx => _dashPressed = true;

        _actions.Enable();
    }
}