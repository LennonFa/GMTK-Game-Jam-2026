using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraSway : MonoBehaviour
{
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float swayAngle = 2f;
    [SerializeField] private float swaySpeed = 8f;
 
  private void Update()
    {
        float horizontalInput = 0f;

        if (Keyboard.current.aKey.isPressed)
            horizontalInput -= 1f;

        if (Keyboard.current.dKey.isPressed)
            horizontalInput += 1f;
        
        float targetZRotation = -horizontalInput * swayAngle;

        Quaternion targetRotation = Quaternion.Euler(
            0f,
            0f,
            targetZRotation
        );

        cameraRoot.localRotation = Quaternion.Lerp(
            cameraRoot.localRotation,
            targetRotation,
            swaySpeed * Time.deltaTime
        );
    }
}
