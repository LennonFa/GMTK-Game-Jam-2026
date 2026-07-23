using System;
using UnityEngine;

public class PlayerOxygen : MonoBehaviour
{
    [SerializeField] private float maxOxygen = 30f;
    [SerializeField] private float oxygenDrainPerSecond = 1f;
    [SerializeField] private float oxygenRefillPerSecond = 4f;

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

        if (CurrentOxygen <= 0f && !hasRunOut)
        {
            hasRunOut = true;
            Debug.Log("Player has no oxygen left!");
        }

        if (CurrentOxygen > 0f)
        {
            hasRunOut = false;
        }
    }

    public void SetUnderwater(bool isUnderwater)
    {
        IsUnderwater = isUnderwater;
    }

}
