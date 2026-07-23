using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerPause : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset pauseUI;
    [SerializeField] private Canvas UICanvas;
    [SerializeField] private PanelSettings panelSettings;

    private UIDocument UIInstance;

    //Systems to disable
    private PlayerMovement movement;
    private PlayerLook looking;
    private PlayerCrouch crouch;
    private PlayerOxygen oxygen;
    private PlayerHeadbob headbob;
    private PlayerCameraSway sway;
    private PlayerThrowing throwing;

    private List<MonoBehaviour> affectedSystems;

    private bool isPaused;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        looking = GetComponent<PlayerLook>();
        headbob = GetComponent<PlayerHeadbob>();
        crouch = GetComponent<PlayerCrouch>();
        oxygen = GetComponent<PlayerOxygen>();
        sway = GetComponent<PlayerCameraSway>();
        throwing = GetComponent<PlayerThrowing>();

        affectedSystems = new List<MonoBehaviour> { throwing, sway, movement, looking, headbob, crouch, oxygen };
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasReleasedThisFrame)
        {
            isPaused = !isPaused;
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (isPaused)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
    }

    void Pause()
    {
        affectedSystems.ForEach(system => system.enabled = false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var canvasChild = new GameObject();
        canvasChild.transform.SetParent(UICanvas.transform, false);
        UIInstance = canvasChild.AddComponent<UIDocument>();
        UIInstance.visualTreeAsset = pauseUI;
        UIInstance.panelSettings = panelSettings;
        SetupPauseMenu();
    }

    void UnPause()
    {
        affectedSystems.ForEach(system => system.enabled = true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TeardownPauseMenu();
        Destroy(UIInstance.gameObject);
    }

    void SetupPauseMenu()
    {
        var root = UIInstance.rootVisualElement;

        var resume = root.Q<Button>("Resume");
        var restart = root.Q<Button>("Restart");
        var quit = root.Q<Button>("Quit");


        if (resume != null)
            resume.clicked += Resume;

        if (restart != null)
            restart.clicked += Restart;

        if (quit != null)
            quit.clicked += Quit;
    }

    void TeardownPauseMenu()
    {
        if (!isPaused) return;
        var root = UIInstance.rootVisualElement;

        var resume = root.Q<Button>("Resume");
        var restart = root.Q<Button>("Restart");
        var quit = root.Q<Button>("Quit");

        if (resume != null)
            resume.clicked -= Resume;

        if (restart != null)
            restart.clicked -= Restart;

        if (quit != null)
            quit.clicked -= Quit;
    }

    void Resume()
    {
        isPaused = false;
        UnPause();
    }

    void Restart()
    {
        // modify this into something robust later
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Quit()
    {
        //modify into something robust later
        #if UNITY_EDITOR
                // Stops Play Mode inside the Unity Editor
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    // Closes the standalone application build
                    // Application.Quit();
        #endif
    }
}
