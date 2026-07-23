using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerState))]
public class PlayerThrowing : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private float maxThrowForce = 3;
    [SerializeField] private float minThrowForce = 1;

    /// <summary>
    /// in seconds
    /// </summary>
    [SerializeField] private float maxChargeTime;

    [SerializeField] private ThrownProjectile projectilePrefab;

    private float currentChargeTime;
    private bool isCharging;
    private PlayerState state;

    private void Awake()
    {
        if (maxThrowForce < minThrowForce)
        {
            throw new Exception("invalid throw force bounds");
        }

        if (!playerCam && !TryGetComponent(out playerCam))
            throw new MissingComponentException("no playerCam");

        if (!state && !TryGetComponent(out state))
            throw new MissingComponentException("no state component");
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            isCharging = true;
            currentChargeTime += Time.deltaTime;
            if (currentChargeTime > maxChargeTime) currentChargeTime = maxChargeTime;
        }
        else
        {
            if (isCharging)
            {
                Debug.Log($"Attempting throw at {currentChargeTime} sec of charge vs {maxChargeTime} s Max");
                if (state.HeldItem != null)
                {
                    Throw();
                }
                isCharging = false;
                currentChargeTime = 0;
            }
        }
    }

    private void Throw()
    {
        var item = (Component)state.TakeHeld();

        var direction = playerCam.transform.forward;

        var chargeFactor = currentChargeTime / maxChargeTime;
        var throwVariance = maxThrowForce - minThrowForce;
        var throwCharge = throwVariance * chargeFactor;
        var throwForce = minThrowForce + throwCharge;

        var projectile = Instantiate(projectilePrefab, playerCam.transform.position, playerCam.transform.rotation);
        projectile.Throw(direction,throwForce, chargeFactor >= 0.5);
        projectile.payload = item.gameObject;

        item.transform.SetParent(projectile.transform, false);
        item.transform.localPosition = Vector3.zero;

    }
}
