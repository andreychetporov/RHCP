public class PlayerInputReader : IPlayerInput
{
    private readonly PlayerInputActions _actions;

    private bool _jumpPressed;
    private bool _jumpReleased;
    private bool _dashPressed;
    private bool _mousePressed;
    private bool _ultaPressed;
    public bool JumpHeld => _actions.Player.Jump.IsPressed();

    public bool JumpPressed => _jumpPressed;
    public bool JumpReleased => _jumpReleased;
    public bool MousePressed => _mousePressed;
    public bool UltaPressed => _ultaPressed;

    public void ConsumeJumpPressed() => _jumpPressed = false;
    public void ConsumeJumpReleased() => _jumpReleased = false;

    public PlayerInputReader()
    {
        _actions = new PlayerInputActions();

        _actions.Player.Jump.performed += ctx => _jumpPressed = true;
        _actions.Player.Jump.canceled += ctx => _jumpReleased = true;
        _actions.Player.Mouse.started += ctx => _mousePressed = true;
        _actions.Player.Mouse.canceled += ctx => _mousePressed = false;
        _actions.Player.Ulta.started += ctx => _ultaPressed = true;
        _actions.Player.Ulta.canceled += ctx => _ultaPressed = false;

        _actions.Enable();
    }
}