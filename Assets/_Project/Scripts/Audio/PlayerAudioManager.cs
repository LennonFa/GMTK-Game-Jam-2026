using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public static PlayerAudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Player Audio Manager in the scene.");
        }
        instance = this;
    } 
}
