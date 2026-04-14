using System.Diagnostics;
using System.Numerics;
using UnityEngine.InputSystem;

public class PlayerInputReader : IPlayerInput
{
    private readonly PlayerInputActions _actions;

    private bool _jumpPressed;
    private bool _jumpReleased;
    private bool _dashPressed;
    private bool _mousePressed;
    public float MoveX => _actions.Player.Move.ReadValue<float>();
    public bool JumpHeld => _actions.Player.Jump.IsPressed();
    public bool CrouchHeld => _actions.Player.Crouch.IsPressed();

    public bool JumpPressed => _jumpPressed;
    public bool JumpReleased => _jumpReleased;
    public bool DashPressed => _dashPressed;

    public bool MousePressed => _mousePressed;

    public void ConsumeJumpPressed() => _jumpPressed = false;
    public void ConsumeJumpReleased() => _jumpReleased = false;
    public void ConsumeDashPressed() => _dashPressed = false;

    public PlayerInputReader()
    {
        _actions = new PlayerInputActions();

        _actions.Player.Jump.performed += ctx => _jumpPressed = true;
        _actions.Player.Jump.canceled += ctx => _jumpReleased = true;
        _actions.Player.Dash.performed += ctx => _dashPressed = true;
        _actions.Player.Mouse.started += ctx => _mousePressed = true;
        _actions.Player.Mouse.canceled += ctx => _mousePressed = false;

        _actions.Enable();
    }
}