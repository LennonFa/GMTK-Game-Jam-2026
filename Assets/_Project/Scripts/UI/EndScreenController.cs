using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using NUnit.Framework;

public class EndScreenController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject endScreenPanel;
    [SerializeField] private TMP_Text rescuredText;
    [SerializeField] private TMP_Text drownedText;

    [Header("Player systems")]
    [SerializeField] private MonoBehaviour[] systemsToDisable;

    [Header("Scenes")]
    [SerializeField] private string mainMenueSceneName = "TitleScreen";

    private ShipSinker shipSinker;
    private Survivor[] passengers;
    private bool hasEnded;
    private PlayerLook playerLook;
    private PlayerMovement playerMovement;
    private PlayerPause playerPause;

    private void Awake()
    {
      Time.timeScale = 1f; 

      if (endScreenPanel != null)
        endScreenPanel.SetActive(false);  
    }

    private void Start()
    {
        playerLook = FindFirstObjectByType<PlayerLook>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        playerPause = FindFirstObjectByType<PlayerPause>();
        shipSinker = FindFirstObjectByType<ShipSinker>();  

        passengers = FindObjectsByType<Survivor>(FindObjectsSortMode.None);

        if (shipSinker == null)
        {
            Debug.LogError("EndSCreenController could not find a ShipSinke.", this);

            enabled = false;
        }
    }

    private void Update()
    {
        if (hasEnded)
            return;

        if (shipSinker.HasFinishedMainSink)
            EndGame();
    }

    private void EndGame()
    {
        if (playerLook != null)
            playerLook.enabled = false;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerPause != null)
            playerPause.enabled = false;

        hasEnded = true;

        int rescuedPassengers = 0;

        foreach (Survivor passenger in passengers)
        {
            if (passenger != null && passenger.isRescued)
                rescuedPassengers++;
        }

        int totalPassengers = passengers.Length;
        int drownedPassengers = totalPassengers - rescuedPassengers;

        rescuredText.text = $"SAVED\n{rescuedPassengers} / {totalPassengers}";


        drownedText.text = $"LOST\n{drownedPassengers}";

        foreach (MonoBehaviour system in systemsToDisable)
        {
            if (system != null && system != this)
                system.enabled = false;
        }

        endScreenPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenueSceneName);
    }
}
