using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCrouch))]
[RequireComponent(typeof(PlayerWaterState))]
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

    [Header("Wading")] //Waterlevels until... 
    [SerializeField] private float wadeSlowStartImmersion = 0.15f;

    [Range(0f, 1f)]
    [SerializeField] private float deepWadeSpeedMultiplier = 0.5f;

    [Range(0f, 1f)]
    [SerializeField] private float sprintDisableImmersion = 0.35f;

    [Range(0f, 1f)]
    [SerializeField] private float jumpDisableImmersion = 0.45f;
    // ...

    [Header("Swimming")]
    [SerializeField] private float surfaceSwimSpeed = 3.5f;
    [SerializeField] private float swimAcceleration = 8;

    [SerializeField] private float surfaceHeadHeight = 0.12f;
    [SerializeField] private float surfaceBuoyancyStrength = 6f;

    [SerializeField] private float swimVerticalAcceleration = 8f;
    [SerializeField] private float maxSwimVerticalSpeed = 2f;
    [SerializeField] private float passiveBuoyancySpeed = 0.25f;

    private Vector3 currentHorizontalVelocity;
    private float verticalVelocity;
    private CharacterController characterController;
    private PlayerCrouch playerCrouch;
    private PlayerWaterState playerWaterState;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCrouch = GetComponent<PlayerCrouch>();
        playerWaterState = GetComponent<PlayerWaterState>();
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

        Vector3 inputDirection = transform.right * input.x + transform.forward * input.y;

        inputDirection = inputDirection.normalized;

        WaterMovementState waterState = playerWaterState.CurrentState;

        bool isWading = waterState == WaterMovementState.Wading;

        bool isSwimming = waterState == WaterMovementState.SurfaceSwimming || waterState == WaterMovementState.Diving;

        float wadeAmount = 0f; 

        if (isWading)
        {
            wadeAmount = Mathf.InverseLerp( //wadeAmount between 0 and 1;   0 = no resistance
                wadeSlowStartImmersion,
                playerWaterState.SwimEnterImmersion,
                playerWaterState.Immersion
            );
        }

        bool canSprint = !playerCrouch.IsCrouching && !isSwimming && (!isWading || playerWaterState.Immersion < sprintDisableImmersion);

        bool isSprinting = Keyboard.current.leftShiftKey.isPressed && canSprint;


        //set movespeed
        float currentSpeed; 

        if (waterState == WaterMovementState.SurfaceSwimming)
        {
            currentSpeed = surfaceSwimSpeed;
        }
        else if (waterState == WaterMovementState.Diving)
        {
            currentSpeed = underwaterMoveSpeed;
        }
        else if (playerCrouch.IsCrouching)
        {
            currentSpeed = crouchMoveSpeed;
        }
        else if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        if (isWading)
        {
            float speedMultiplier = Mathf.Lerp(1f, deepWadeSpeedMultiplier, wadeAmount);

            currentSpeed *= speedMultiplier;
        }

        

        Vector3 targetVelocity = inputDirection * currentSpeed;


        float changeSpeed;

        if (isSwimming)
        {
            changeSpeed = swimAcceleration;
        }
        else
        {
            changeSpeed = inputDirection == Vector3.zero ? deceleration : acceleration;
        }

        currentHorizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, targetVelocity, changeSpeed * Time.deltaTime);

        
        if (isSwimming)
        {
            HandleSwimmingVerticalMovement(waterState);
        }
        else
        {
            if (characterController.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            bool canJump = !isWading || playerWaterState.Immersion < jumpDisableImmersion;

            if (characterController.isGrounded && canJump && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 finalVelocity = currentHorizontalVelocity;
        finalVelocity.y = verticalVelocity;

        characterController.Move(finalVelocity * Time.deltaTime);
    }

    private void HandleSwimmingVerticalMovement(WaterMovementState waterState)
    {
        bool wantsToRise = Keyboard.current.spaceKey.isPressed;

        bool wantsToDive = Keyboard.current.leftCtrlKey.isPressed;

        float targetVerticalVelocity;

        if (waterState == WaterMovementState.SurfaceSwimming && !wantsToDive)
        {
            float targetHeadDepth = -surfaceHeadHeight;
            float depthError = playerWaterState.HeadDepth - targetHeadDepth;

            targetVerticalVelocity = Mathf.Clamp(depthError *surfaceBuoyancyStrength, -maxSwimVerticalSpeed, maxSwimVerticalSpeed);
        }
        else
        {
            float verticalInput = 0f;

            if (wantsToRise)
                verticalInput += 1f;

            if (wantsToDive)
                verticalInput -= 1f;

            if (verticalInput == 0f)
            {
                targetVerticalVelocity = passiveBuoyancySpeed;
            }
            else
            {
                targetVerticalVelocity = verticalInput * maxSwimVerticalSpeed;
            }
        }
        
        verticalVelocity = Mathf.MoveTowards(verticalVelocity, targetVerticalVelocity, swimVerticalAcceleration * Time.deltaTime);
    }
}
