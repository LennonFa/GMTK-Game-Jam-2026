using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerState))] // interaction might affect state
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private GameObject interactionPrompt;

    private PlayerState playerState; // interaction might depend on state

    private void Awake()
    {
        if (!playerState && !TryGetComponent(out playerState))
            throw new MissingComponentException("no playerState component attached");
    }

    private void Update()
    {
        bool foundInteractable = TryGetInteractable(out IInteractable interactable);

        interactionPrompt.SetActive(foundInteractable);

        if (foundInteractable && Keyboard.current.eKey.wasPressedThisFrame)
        {
            interactable.Interact(playerState);
        }
    }
    private bool TryGetInteractable(out IInteractable interactable)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            interactable = hit.collider.GetComponentInParent<IInteractable>();

            return interactable != null;
        }

        interactable = null;
        return false;
    }
}
