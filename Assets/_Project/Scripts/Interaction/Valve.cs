using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Valve : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onActivated;

    private bool isActivated;

    public void Interact(PlayerState state)
    {
        if (isActivated)
            return;

        isActivated = true;

        Debug.Log(gameObject.name + " activated!");

        onActivated.Invoke();
    }
}
  
