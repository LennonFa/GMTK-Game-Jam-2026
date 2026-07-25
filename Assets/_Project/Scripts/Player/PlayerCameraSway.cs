using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerWaterState))]
public class PlayerCameraSway : MonoBehaviour
{
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float swayAngle = 2f;
    [SerializeField] private float swaySpeed = 8f;

    [Header("Swimming")]
    [SerializeField] private float swimRollAmount = 1.2f;
    [SerializeField] private float swimPitchAmount = 0.6f;
    [SerializeField] private float swimSwaySpeed = 1.5f;

    private PlayerWaterState playerWaterState;
 
    private void Awake()
    {
        playerWaterState = GetComponent<PlayerWaterState>();
    }

  private void Update()
    {
        float horizontalInput = 0f;

        if (Keyboard.current.aKey.isPressed)
            horizontalInput -= 1f;

        if (Keyboard.current.dKey.isPressed)
            horizontalInput += 1f;
        
        float targetZRotation = -horizontalInput * swayAngle;

        float targetXRotation = 0f;

        WaterMovementState waterState = playerWaterState.CurrentState;

        bool isSwimming = waterState == WaterMovementState.SurfaceSwimming || waterState == WaterMovementState.Diving;

        if (isSwimming)
        {
            targetZRotation += Mathf.Sin(Time.time * swimSwaySpeed) * swimRollAmount;

            targetXRotation = Mathf.Sin(Time.time * swimSwaySpeed * 0.7f) * swimPitchAmount;
        }

        Quaternion targetRotation = Quaternion.Euler(targetXRotation, 0f, targetZRotation);

        cameraRoot.localRotation = Quaternion.Lerp(cameraRoot.localRotation, targetRotation, swaySpeed * Time.deltaTime);
    }
}
