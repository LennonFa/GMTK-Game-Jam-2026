using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference Storm { get; private set; }
    [field: SerializeField] public EventReference WaterAmbience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference Jump { get; private set; }
    [field: SerializeField] public EventReference Wade { get; private set; }
    [field: SerializeField] public EventReference SwimShallow { get; private set; }
    [field: SerializeField] public EventReference Swim { get; private set; }
    [field: SerializeField] public EventReference DryWalk { get; private set; }
    [field: SerializeField] public EventReference DryRun { get; private set; }

    [field: Header("UI SFX")]
    [field: SerializeField] public EventReference UIHover { get; private set; }
    [field: SerializeField] public EventReference UIAccept { get; private set; }


    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
