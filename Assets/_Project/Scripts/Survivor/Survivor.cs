using System;
using NUnit.Framework;
using UnityEngine;

public class Survivor : MonoBehaviour, IInteractable, IHoldable
{
    private bool isRescued;
    private bool isHeld;

    private Vector3 velocity;

    public void Interact(PlayerState state)
    {
        if (isRescued)
        {
            Debug.Log("Passenger is already rescued");
            return;
        }

        if (isHeld) {
            Debug.Log("Tried to grab item in hand");
            return;
        }

        if (state.HeldItem != null)
        {
            Debug.Log($"can't grab {name}, player's hands full");
            return;
        }

        isHeld = true;
        state.HeldItem = this;

        Hold(state.gameObject, state.holdingPosition);

        Debug.Log(gameObject.name + "Grabbed");
    }

    public void Hold(GameObject parent, Transform holdingPosition)
    {
        ThrownProjectile projectile = GetComponentInParent<ThrownProjectile>();

        if (projectile != null)
        {
            projectile.TakePayload();
            Destroy(projectile.gameObject);
        }

        isHeld = true;

        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = holdingPosition.position;
        gameObject.transform.rotation = holdingPosition.rotation;
    }

    public void Release()
    {
        isHeld = false;
        Debug.Log("Release got triggered "); //Remove!!!
    }

    public void Rescue()
    {
        isRescued = true;
        isHeld = false;

        int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");

        SetLayerRecursively(gameObject, ignoreRaycastLayer);
    }

    private void SetLayerRecursively(GameObject target, int layer)
    {
        target.layer = layer;

        foreach (Transform child in target.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}

