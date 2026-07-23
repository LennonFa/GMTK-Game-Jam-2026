using System;
using UnityEngine;

public class PlayerOxygen : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 30f;
    [SerializeField] private float oxygenDrainPerSecond = 1f;
    [SerializeField] private float oxygenRefillPerSecond = 4f;
    [SerializeField] private float lowOxygenThreshold = 0.25f;

    public event Action OxygenDepleted;

    public bool IsLowOnOxygen =>
        OxygenNormalized <= lowOxygenThreshold;
    public bool IsOutOfOxygen => 
        CurrentOxygen <= 0f;
    private bool lowOxygenWarningActive;
    private bool hasRunOut;

    public float OxygenNormalized =>
        maxOxygen <= 0f ? 0f : CurrentOxygen / maxOxygen;
    public float CurrentOxygen { get; private set; }
    public bool IsUnderwater { get; private set; }

    private void Awake()
    {
        CurrentOxygen = maxOxygen;
    }

    private void Update()
    {
        //Debug.Log("Current oxygen: " + CurrentOxygen);

        if (IsUnderwater)
        {
            CurrentOxygen -= oxygenDrainPerSecond * Time.deltaTime;
        }
        else
        {
            CurrentOxygen += oxygenRefillPerSecond * Time.deltaTime;
        }

        CurrentOxygen = Mathf.Clamp(
            CurrentOxygen,
            0f,
            maxOxygen
        );

        if (IsOutOfOxygen && !hasRunOut)
        {
            hasRunOut = true;
            OxygenDepleted?.Invoke();
            Debug.Log("Player has no oxygen left!");
        }

        if (!IsOutOfOxygen)
        {
            hasRunOut = false;
        }

        if (CurrentOxygen > 0f)
        {
            hasRunOut = false;
        }

        if (IsLowOnOxygen && !lowOxygenWarningActive)
        {
            lowOxygenWarningActive = true;
            Debug.Log("Low oxygen!");
        }

        //Reset Warning
        if (!IsLowOnOxygen)
        {
            lowOxygenWarningActive = false;
        }
    }

    public void SetUnderwater(bool isUnderwater)
    {
        IsUnderwater = isUnderwater;
    }

}
