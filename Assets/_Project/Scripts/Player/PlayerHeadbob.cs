using System.Threading;
using UnityEngine;

public class PlayerHeadbob : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float walkFrequency = 10f;
    [SerializeField] private float sprintFrequency = 14f;

    [SerializeField] private float walkBobHeight = 0.05f;
    [SerializeField] private float sprintBobHeight = 0.075f;

    [SerializeField] private float bobSideAmount = 0.03f;
    [SerializeField] private float sprintSpeed = 8f;

    [SerializeField] private float bobSmoothingSpeed = 10f;
    
    private CharacterController characterController;
    private Vector3 cameraStartPosition;
    private float bobTimer;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        cameraStartPosition = playerCamera.localPosition;
    }

    private void Update()
    {
        HandleHeadbob();
    }

    private void HandleHeadbob()
    {
        Vector3 horizontalVelocity = characterController.velocity;
        horizontalVelocity.y = 0f;

        bool isMoving = 
            horizontalVelocity.magnitude > 0.1f &&
            characterController.isGrounded;

        Vector3 targetPosition = cameraStartPosition;

        if (isMoving)
        {
            float speedPercentage =
                Mathf.Clamp01(horizontalVelocity.magnitude / sprintSpeed);

            float currentFrequency = 
                Mathf.Lerp(walkFrequency, sprintFrequency, speedPercentage);

            float currentBobHeight = 
                Mathf.Lerp(walkBobHeight, sprintBobHeight, speedPercentage);

            bobTimer += Time.deltaTime * currentFrequency;

            float verticalOffset =
                Mathf.Sin(bobTimer) * currentBobHeight;

            float horizontalOffset = 
                Mathf.Sin(bobTimer * 0.5f) * bobSideAmount;

            targetPosition += new Vector3(
                horizontalOffset,
                verticalOffset,
                0f
            );
        }
        else
        {
            bobTimer = 0f;
        }

        playerCamera.localPosition = Vector3.Lerp(
            playerCamera.localPosition,
            targetPosition,
            bobSmoothingSpeed * Time.deltaTime
        );
    }
}
