using UnityEngine;
using UnityEngine.UI;
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

    [Header("UI Reference")]
    [SerializeField] private Image hud;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float minRotateVelocity = 0.05f;
    [SerializeField] private Vector3 modelForwardAxis = Vector3.right;

    [SerializeField] private WeaponSO weapon;

    [Inject] private IPlayerInput _input;

    private bool isSlicing = false;

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

    private void Start()
    {
        EquipWeapon(weapon);
    }

    public void EquipWeapon(WeaponSO newWeapon)
    {
        weapon = newWeapon;
        hud.transform.localScale = new Vector3(weapon.DamageRadius, weapon.DamageRadius, 0.0f);
        slice.SetWeapon(newWeapon);
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        bool jumpPressed = _input.JumpPressed;
        bool jumpReleased = _input.JumpReleased;
        bool jumpHeld = _input.JumpHeld;

        if (jumpPressed)
            _input.ConsumeJumpPressed();

        if (jumpReleased)
            _input.ConsumeJumpReleased();

        groundProbe.TickProbe();
        hoverSuspension.TickSuspension();

        jumpMotor.TickJump(jumpPressed, jumpReleased, jumpHeld, dt);

        movementMotor.TickMove(dt);

        RotateCharacter();
    }

    private void Update() => ManageSliceState();

    private void ManageSliceState()
    {
        if (_input.MousePressed && !isSlicing)
        {
            slice.Reset();
            isSlicing = true;
            slice.SetEmitting(true);
        }

        bool isCursorInRange = RectTransformUtility.RectangleContainsScreenPoint(hud.rectTransform, Mouse.current.position.ReadValue(), null);
        if (!isCursorInRange)
        {
            isSlicing = false;
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