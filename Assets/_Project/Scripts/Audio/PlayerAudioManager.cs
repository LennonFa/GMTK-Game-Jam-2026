using FMOD.Studio;
using FMODUnity;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public static PlayerAudioManager instance { get; private set; }
    [SerializeField] public EventInstance CurrentMovement;
    [SerializeField] private EventReference CurrentRef;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Player Audio Manager in the scene.");
        }
        instance = this;
    }

    public void SwitchMovementSFX(FMODUnity.EventReference SFX)
    {
        if (CurrentRef.Guid == SFX.Guid)
        {
            return;
        }
        CurrentRef = SFX;
        CurrentMovement.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        CurrentMovement = AudioManager.instance.CreateInstance(SFX);
        CurrentMovement.start();
    }

}
