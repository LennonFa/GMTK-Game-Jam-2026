using System;
using NUnit.Framework;
using UnityEngine;

public class Survivor : MonoBehaviour, IInteractable, IHoldable
{
    private bool isRescued;
    private bool isHeld;
    private bool isThrown;

    private Vector3 direction;

    private void Update()
    {
        if (isThrown)
        {
            UpdatePosition();
        }

    }

    private void UpdatePosition()
    {


    }

    public void Interact(PlayerState state)
    {
        if (isHeld)
            return;

        isHeld = true;
        state.HeldItem = this;

        Hold(state.gameObject, new Vector3(0.3f, 1f, 0.3f));

        Debug.Log(gameObject.name + "Grabbed");
    }

    public void Hold(GameObject parent, Vector3 holdingPosition)
    {
        isHeld = true;
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.localPosition = holdingPosition;

        //theres gotta be a better way to do this
        var rotation = gameObject.transform.rotation;
        var angles = rotation.eulerAngles;
        angles.x = 90;
        rotation.eulerAngles = angles;

        gameObject.transform.rotation = rotation;
    }

    public void Release()
    {
        isHeld = false;
        isThrown = true;
    }
}

