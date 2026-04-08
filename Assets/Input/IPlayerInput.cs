public interface IPlayerInput
{
    float MoveX { get; }

    bool JumpPressed { get; }
    bool JumpReleased { get; }
    bool JumpHeld { get; }

    void ConsumeJumpPressed();
    void ConsumeJumpReleased();
}