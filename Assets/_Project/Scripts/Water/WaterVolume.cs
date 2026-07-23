using UnityEngine;

public class WaterVolume : MonoBehaviour
{
  [SerializeField] private Transform waterSurface;

  private PlayerOxygen playerOxygen;
  private Transform playerCamera;

  private void Update()
    {
        if (playerOxygen == null)
            return;

        bool isUnderwater = 
            playerCamera.position.y < waterSurface.position.y;

        playerOxygen.SetUnderwater(isUnderwater);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerOxygen oxygen))
            return;

        playerOxygen = oxygen;
        playerCamera = 
            other.GetComponentInChildren<Camera>().transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out PlayerOxygen oxygen))
            return;

        oxygen.SetUnderwater(false);

        playerOxygen = null;
        playerCamera = null;
    }
}
