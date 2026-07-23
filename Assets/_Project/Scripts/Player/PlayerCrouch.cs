using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouch : MonoBehaviour
{
    [SerializeField] private Transform cameraRoot;

    [SerializeField] private float standingHeight = 1.8f;
    [SerializeField] private float crouchingHeight = 1.1f;

    [SerializeField] private float standingCameraHeight = 1.6f;
    [SerializeField] private float crouchingCameraHeight = 0.9f;

    [SerializeField] private float crouchSpeed = 8f;

    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float ceilingCheckRadius = 0.3f;

    private CharacterController characterController;
    public bool IsCrouching { get; private set; }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleCrouch();
    }

    private void HandleCrouch()
    {
        bool crouchInput = 
            Keyboard.current.leftCtrlKey.isPressed;

        if (!crouchInput && IsCrouching && !CanStandUp())
        {
            crouchInput = true;
        }

        IsCrouching = crouchInput;

        float targetHeight = 
            IsCrouching ? crouchingHeight : standingHeight;

        float targetCameraHeight = 
            IsCrouching ? crouchingCameraHeight : standingCameraHeight;

        characterController.height = Mathf.MoveTowards(characterController.height, targetHeight, crouchSpeed * Time.deltaTime);

        Vector3 controllerCenter = characterController.center;
        controllerCenter.y = characterController.height / 2f;
        characterController.center = controllerCenter;

        Vector3 cameraPosition = cameraRoot.localPosition;

        cameraPosition.y = Mathf.MoveTowards(cameraPosition.y, targetCameraHeight, crouchSpeed * Time.deltaTime);

        cameraRoot.localPosition = cameraPosition;
    }

    private bool CanStandUp()
    {
        float checkRadius = 
            characterController.radius * 0.9f;

        float distanceToStanding =
            standingHeight - characterController.height;

        if (distanceToStanding <= 0f)
            return true;

        Vector3 checkStart =
            transform.position + 
            Vector3.up * (characterController.height - checkRadius);

        bool ceilingDetected = Physics.SphereCast(
            checkStart,
            checkRadius,
            Vector3.up,
            out _,
            distanceToStanding,
            obstacleLayer,
            QueryTriggerInteraction.Ignore
        );

        return !ceilingDetected;
    }
}
