using UnityEngine;

[RequireComponent(typeof(PlayerOxygen))]
public class PlayerDeath : MonoBehaviour
{
   private PlayerOxygen playerOxygen;
   private bool isDead;

   private void Awake()
    {
        playerOxygen = GetComponent<PlayerOxygen>();
    }

    private void OnEnable()
    {
        playerOxygen.OxygenDepleted += HandleOxygenDepleted;
    }

    private void OnDisable()
    {
        playerOxygen.OxygenDepleted -= HandleOxygenDepleted;
    }

    private void HandleOxygenDepleted()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log("Game Over: Player drowned!");
    }
}
