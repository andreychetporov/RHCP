using System.Numerics;

public interface IPlayerInput
{
    float MoveX { get; }
    bool JumpPressed { get; }
    bool JumpReleased { get; }
    bool JumpHeld { get; }
    bool CrouchHeld { get; }
    bool DashPressed { get; }
    bool MouseHeld { get; }

    void ConsumeJumpPressed();
    void ConsumeJumpReleased();

    void ConsumeDashPressed();
}