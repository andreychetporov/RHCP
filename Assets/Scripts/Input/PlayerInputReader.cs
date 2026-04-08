using UnityEngine.InputSystem;

public class PlayerInputReader : IPlayerInput
{
    private readonly PlayerInputActions _actions;

    private bool _jumpPressed;
    private bool _jumpReleased;

    public float MoveX => _actions.Player.Move.ReadValue<float>();
    public bool JumpHeld => _actions.Player.Jump.IsPressed();

    public bool JumpPressed => _jumpPressed;
    public bool JumpReleased => _jumpReleased;

    public void ConsumeJumpPressed() => _jumpPressed = false;
    public void ConsumeJumpReleased() => _jumpReleased = false;

    public PlayerInputReader()
    {
        _actions = new PlayerInputActions();
        _actions.Player.Jump.performed += ctx => _jumpPressed = true;
        _actions.Player.Jump.canceled += ctx => _jumpReleased = true;
        _actions.Enable();
    }
}