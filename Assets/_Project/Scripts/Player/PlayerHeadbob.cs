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

    [Header("Wading")]
    [SerializeField] private float wadePeakImmersion = 0.45f;
    [SerializeField] private float wadePeakHeightMultiplier = 1.5f;
    [SerializeField] private float deepWadeHeightMultiplier = 0.6f;
    [SerializeField] private float deepWadeFrequencyMultiplier = 0.5f;
    [SerializeField] private float deepWadeSideMultiplier = 1.35f;
    
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

        bool isWading = waterState == WaterMovementState.Wading;

        bool isSwimming = waterState == WaterMovementState.SurfaceSwimming || waterState == WaterMovementState.Diving;
        bool isMoving = horizontalVelocity.magnitude > 0.1f && characterController.isGrounded && !isSwimming;

        if (isMoving)
        {
            float speedPercentage = Mathf.Clamp01(horizontalVelocity.magnitude / sprintSpeed);

            float currentFrequency = Mathf.Lerp(walkFrequency, sprintFrequency, speedPercentage);

            float currentBobHeight = Mathf.Lerp(walkBobHeight, sprintBobHeight, speedPercentage);

            float currentSideAmount = bobSideAmount;


            if (isWading)
            {
                float immersion = playerWaterState.Immersion;

                float totalWadeAmount = Mathf.InverseLerp(0f, playerWaterState.SwimEnterImmersion, immersion);

                float heightMultiplier;

                if (immersion <= wadePeakImmersion)
                {
                    float riseAmount = Mathf.InverseLerp(0f, wadePeakImmersion, immersion);

                    heightMultiplier = Mathf.Lerp(1f, wadePeakHeightMultiplier, riseAmount);
                }
                else
                {
                    float fallAmount = Mathf.InverseLerp(wadePeakImmersion, playerWaterState.SwimEnterImmersion, immersion);

                    heightMultiplier = Mathf.Lerp(wadePeakHeightMultiplier, deepWadeHeightMultiplier, fallAmount);
                }

                currentBobHeight *= heightMultiplier;

                currentFrequency *= Mathf.Lerp(1f, deepWadeFrequencyMultiplier, totalWadeAmount);

                currentSideAmount *= Mathf.Lerp(1f, deepWadeSideMultiplier, totalWadeAmount);
            }

            bobTimer += Time.deltaTime * currentFrequency;

            float verticalOffset = Mathf.Sin(bobTimer) * currentBobHeight;

            float horizontalOffset = Mathf.Sin(bobTimer * 0.5f) * currentSideAmount;

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
