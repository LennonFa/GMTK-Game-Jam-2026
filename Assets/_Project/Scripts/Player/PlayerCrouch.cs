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
        IsCrouching = 
            Keyboard.current.leftCtrlKey.isPressed;

        float targetHeight = 
            IsCrouching ? crouchingHeight : standingHeight;

        float targetCameraHeight = 
            IsCrouching ? crouchingCameraHeight : standingCameraHeight;

        characterController.height = Mathf.MoveTowards(
            characterController.height,
            targetHeight,
            crouchSpeed * Time.deltaTime
        );

        Vector3 cameraPosition = cameraRoot.localPosition;

        cameraPosition.y = Mathf.MoveTowards(
            cameraPosition.y,
            targetCameraHeight,
            crouchSpeed * Time.deltaTime
        );

        cameraRoot.localPosition = cameraPosition;
    }
}
