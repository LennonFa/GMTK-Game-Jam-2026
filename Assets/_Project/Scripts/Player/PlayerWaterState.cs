using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum WaterMovementState
{
    Dry,
    Wading,
    SurfaceSwimming,
    Diving
}

[RequireComponent(typeof(CharacterController))]
public class PlayerWaterState : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCamera;

    [Header("Swimming thresholds")]
    [Range(0f, 1f)]
    [SerializeField] private float swimEnterImmersion = 0.72f;

    [Range(0f,1f)]
    [SerializeField] private float swimExitImmersion = 0.60f;

    [Header("Diving threshold")]
    [SerializeField] private float diveEnterDepth = 0.05f;
    [SerializeField] private float diveExitHeight = 0.10f;

    public WaterMovementState CurrentState { get; private set; }

    public bool IsInWater => waterSurface != null;

    public float SwimEnterImmersion => swimEnterImmersion;
    public float WaterDepth { get; private set; }
    public float Immersion { get; private set; }
    public float HeadDepth { get; private set; }

    private CharacterController characterController;
    private Transform waterSurface; 
    private float standingHeight;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        standingHeight = characterController.height;

        CurrentState = WaterMovementState.Dry;
    }

    private void Update()
    {
        UpdateMeasurements(); 
        UpdateWaterState();
    }

    public void EnterWater(Transform surface)
    {
        waterSurface = surface;
    }

    public void ExitWater(Transform surface)
    {
        if (waterSurface != surface)
            return;

        waterSurface = null;

        WaterDepth = 0f;
        Immersion = 0f;
        HeadDepth = 0f;

        SetState(WaterMovementState.Dry);
    }

    private void UpdateMeasurements()
    {
        if (!IsInWater)
            return;

        float surfaceY = waterSurface.position.y;

        float feetY = characterController.bounds.min.y;

        WaterDepth = Mathf.Max(0f, surfaceY - feetY);

        Immersion = Mathf.Clamp01(WaterDepth / standingHeight);

        HeadDepth = surfaceY - playerCamera.position.y;
    }

    private void UpdateWaterState()
    {
        if (!IsInWater)
        {
            SetState(WaterMovementState.Dry);
            return;
        }

        bool shouldDive;

        if (CurrentState == WaterMovementState.Diving)
        {
            shouldDive = HeadDepth > -diveExitHeight;
        }
        else
        {
            shouldDive = HeadDepth > diveEnterDepth;
        }

        if (shouldDive)
        {
            SetState(WaterMovementState.Diving);
            return;
        }

        bool shouldSwim;

        if (CurrentState == WaterMovementState.SurfaceSwimming)
        {
            shouldSwim = Immersion >= swimExitImmersion;
        }
        else
        {
            shouldSwim = Immersion >= swimEnterImmersion;
        }

        SetState(shouldSwim ? WaterMovementState.SurfaceSwimming : WaterMovementState.Wading);
    }

    private void SetState(WaterMovementState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        Debug.Log("Water state: " + CurrentState);
    }
}
