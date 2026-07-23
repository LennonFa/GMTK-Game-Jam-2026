using System.Collections.Generic;
using UnityEngine;

public class Lifeboat : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Transform> seats;
    private bool isFull;
    private int nextSeatIndex;

    public void Seat(GameObject passenger)
    {
        var seatPosition = seats[nextSeatIndex];

        // in case the boat bobs, link transforms
        passenger.transform.SetParent(transform);
        passenger.transform.position = seatPosition.position;
        passenger.transform.rotation = seatPosition.rotation;

        nextSeatIndex++;
        if (nextSeatIndex >= seats.Count)
        {
            isFull = true;
        }
    }

    public void Interact(PlayerState state)
    {
        Debug.Log($"{name} Interacted");
        if (state.HeldItem != null && !isFull)
        {
            var item = (Component)state.HeldItem;
            if (item.TryGetComponent<Survivor>(out var survivor))
            {
                Seat(survivor.gameObject);
                survivor.Rescue();
            }
        }
    }
}
