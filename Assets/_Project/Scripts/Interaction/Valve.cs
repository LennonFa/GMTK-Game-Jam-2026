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

        isActivated = true;

        Debug.Log(gameObject.name + " activated!");

        onActivated.Invoke();

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayOneShot(openSound, transform.position);
        }
        else
        {
            Debug.LogWarning("NoAudioManager found. valve sound skipped.");
        }
    }
}
  
