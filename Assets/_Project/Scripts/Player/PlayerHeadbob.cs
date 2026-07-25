using UnityEngine;

[RequireComponent(typeof(PlayerWaterState))]
public class PlayerHeadbob : MonoBehaviour
{
    [SerializeField] private bool _enable = true;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private float walkFrequency = 10f;
    [SerializeField] private float sprintFrequency = 14f;

    [SerializeField] private float walkBobHeight = 0.05f;
    [SerializeField] private float sprintBobHeight = 0.075f;

    [SerializeField] private float bobSideAmount = 0.03f;
    [SerializeField] private float sprintSpeed = 8f;

    [SerializeField] private float bobSmoothingSpeed = 10f;
    [SerializeField] private float landingBobAmount = 0.12f;
    [SerializeField] private float landingREcoverySpeed = 4f;
    
    private PlayerWaterState playerWaterState;
    private CharacterController characterController;
    private Vector3 cameraStartPosition;
    private float bobTimer;
    private float landingOffset;
    private bool wasGrounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        cameraStartPosition = playerCamera.localPosition;
        playerWaterState = GetComponent<PlayerWaterState>();
    }

    private void Start()
    {
        wasGrounded = characterController.isGrounded;
    }

    private void Update()
    {
        if (!_enable) return;

        HandleHeadbob();
        HandleLanding();
    }

    private void HandleHeadbob()
    {
        Vector3 horizontalVelocity = characterController.velocity;
        horizontalVelocity.y = 0f;

        

        Vector3 targetPosition = cameraStartPosition;
        targetPosition.y += landingOffset;

        WaterMovementState waterState = playerWaterState.CurrentState;

        bool isSwimming = waterState == WaterMovementState.SurfaceSwimming || waterState == WaterMovementState.Diving;
        bool isMoving = horizontalVelocity.magnitude > 0.1f && characterController.isGrounded && !isSwimming;

        if (isMoving)
        {
            float speedPercentage = Mathf.Clamp01(horizontalVelocity.magnitude / sprintSpeed);

            float currentFrequency = Mathf.Lerp(walkFrequency, sprintFrequency, speedPercentage);

            float currentBobHeight = Mathf.Lerp(walkBobHeight, sprintBobHeight, speedPercentage);

            bobTimer += Time.deltaTime * currentFrequency;

            float verticalOffset = Mathf.Sin(bobTimer) * currentBobHeight;

            float horizontalOffset = Mathf.Sin(bobTimer * 0.5f) * bobSideAmount;

            targetPosition += new Vector3(horizontalOffset, verticalOffset, 0f);
        }
        else
        {
            bobTimer = 0f;
        }

        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, targetPosition, bobSmoothingSpeed * Time.deltaTime);
    }
    
    private void HandleLanding()
    {
        WaterMovementState waterState = playerWaterState.CurrentState;
        
        bool isSwimming = waterState == WaterMovementState.SurfaceSwimming || waterState == WaterMovementState.Diving;
        bool isGrounded = characterController.isGrounded;

        if (!wasGrounded && isGrounded && !isSwimming)
        {
            landingOffset = -landingBobAmount;
        }

        landingOffset = Mathf.MoveTowards(landingOffset, 0f, landingREcoverySpeed * Time.deltaTime);

        wasGrounded = isGrounded;
    }
}
