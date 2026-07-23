using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCrouch))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float crouchMoveSpeed = 2.5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 25f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float underwaterMoveSpeed = 3f;

    private Vector3 currentHorizontalVelocity;
    private float verticalVelocity;
    private CharacterController characterController;
    private PlayerCrouch playerCrouch;
    private PlayerOxygen playerOxygen;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCrouch = GetComponent<PlayerCrouch>();
        playerOxygen = GetComponent<PlayerOxygen>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1;

        Vector3 inputDirection =
            transform.right * input.x +
            transform.forward * input.y;

        inputDirection = inputDirection.normalized;

        bool isSprinting =
            Keyboard.current.leftShiftKey.isPressed &&
            !playerCrouch.IsCrouching &&
            !playerOxygen.IsUnderwater;

        float currentSpeed;

        if (playerCrouch.IsCrouching)
        {
            currentSpeed = crouchMoveSpeed;
        }
        else if (playerOxygen.IsUnderwater)
        {
            currentSpeed = underwaterMoveSpeed;
        }
        else if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        Vector3 targetVelocity = inputDirection * currentSpeed;

        float changeSpeed =
            inputDirection == Vector3.zero ? deceleration : acceleration;

        currentHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetVelocity,
            changeSpeed * Time.deltaTime
        );

        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        if (characterController.isGrounded &&
            !playerOxygen.IsUnderwater &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            verticalVelocity = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalVelocity = currentHorizontalVelocity;
        finalVelocity.y = verticalVelocity;

        characterController.Move(finalVelocity * Time.deltaTime);
    }
}
