public interface IPlayerInput
{
    bool JumpPressed { get; }
    bool JumpReleased { get; }
    bool JumpHeld { get; }
    bool MousePressed { get; }
    bool UltaPressed {  get; }
    void ConsumeJumpPressed();
    void ConsumeJumpReleased();
}