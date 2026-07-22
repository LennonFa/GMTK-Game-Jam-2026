using NUnit.Framework;
using UnityEngine;

public class Survivor : MonoBehaviour, IInteractable
{
    private bool isRescued;

    public void Interact()
    {
        if (isRescued)
            return;

        isRescued = true;

        Debug.Log(gameObject.name + "was saved");
    }
}

