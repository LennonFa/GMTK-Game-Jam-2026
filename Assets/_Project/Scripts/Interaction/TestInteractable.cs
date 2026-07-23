using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void Interact(PlayerState state)
    {
        Debug.Log("Interact with: " + gameObject.name + "\n Player State: " + state);
    }
}
