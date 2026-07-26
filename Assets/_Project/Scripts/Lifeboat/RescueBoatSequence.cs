using UnityEngine;
using System.Collections;

public class RescueBoatSequence : MonoBehaviour
{
    [Header("Boats")]
    [SerializeField] private Transform[] boats;

    [Header("Route")]
    [SerializeField] private Transform approachPoint;
    [SerializeField] private Transform dockPoint;
    [SerializeField] private Transform exitPoint;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float waitAtDock = 10f;
    [SerializeField] private float delayBeweenBoats = 2f;

    private void Start()
    {
        StartCoroutine(RunBoatSequence());
    } 

    private IEnumerator RunBoatSequence()
    {
        foreach (Transform boat in boats)
        {
            if (boat == null)
            {
                continue;
            }

           StartCoroutine(RunSingleBoat(boat));

           //distance betwen boats
           yield return new WaitForSeconds(delayBeweenBoats);
        }
    }

    private IEnumerator RunSingleBoat(Transform boat)
    {
        yield return MoveBoatTo(boat, approachPoint.position);
        yield return MoveBoatTo(boat, dockPoint.position);

        yield return new WaitForSeconds(waitAtDock);

        yield return MoveBoatTo(boat, exitPoint.position);

        //afet exitPoint just keep moving
        while (true)
        {
            boat.position += boat.forward * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MoveBoatTo(Transform boat, Vector3 targetPosition)
    {
        while (Vector3.Distance(boat.position, targetPosition) > 0.05f)
        {
            Vector3 direction = targetPosition - boat.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                boat.rotation = Quaternion.LookRotation(direction);
            }

            boat.position = Vector3.MoveTowards(boat.position, targetPosition,  moveSpeed * Time.deltaTime);

            yield return null;
        }

        boat.position = targetPosition;
    }
}
