using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using FMODUnity;

public class Valve : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onActivated;
    [SerializeField] private FMODUnity.EventReference openSound;

    private bool isActivated;

    public void Interact(PlayerState state)
    {
        if (isActivated)
            return;

        AudioManager.instance.PlayOneShot(openSound, this.transform.position);
        isActivated = true;

        Debug.Log(gameObject.name + " activated!");

        onActivated.Invoke();
    }
}
  
