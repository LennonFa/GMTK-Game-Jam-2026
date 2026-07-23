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
        isHeld = true;
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = holdingPosition.position;
        gameObject.transform.rotation = holdingPosition.rotation;
    }

    public void Release()
    {
        isHeld = false;
    }

    public void Rescue()
    {
        isRescued = true;
    }

    public void Rescue()
    {
        isRescued = true;
    }
}

