using UnityEngine;

public class LifeboatCatchZone : MonoBehaviour
{
    [SerializeField] private Lifeboat lifeboat;

    private void OnTriggerEnter(Collider other)
    {
        ThrownProjectile projectile = other.GetComponentInParent<ThrownProjectile>();

        if (projectile == null)
        return;

        lifeboat.CatchThrownPassenger(projectile);
    }

    
}
