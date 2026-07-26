using Unity.VisualScripting;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerWaterState))]
public class UnderwaterVisuals : MonoBehaviour
{
    [UnitHeaderInspectable("Refernces")]
    [SerializeField] private Volume underwaterVolume;

    [Header("Blend Settings")]
    [Min(0.01f)]
    [SerializeField] private float fullEffectDepth = 0.35f;

    [SerializeField] private float fadeSpeed = 3f;

    private PlayerWaterState waterState;

    private void Awake()
    {
        waterState = GetComponent<PlayerWaterState>();

        if (underwaterVolume == null)
            underwaterVolume = GetComponentInChildren<Volume>(true);

        if (waterState == null)
        {
            Debug.LogError("PlayerWaterState missing on Player.", this);
            enabled = false;
            return;
        }

        if (underwaterVolume == null)
        {
            Debug.LogError("Underwater Volume not found.", this);
            enabled = false;
            return;
        }

        underwaterVolume.weight = 0f;
    }

    private void Update()
    {
        if (underwaterVolume == null)
            return;

        float targetWeight = 0f;

        if (waterState.IsInWater && waterState.HeadDepth > 0f)
        {
            targetWeight = Mathf.InverseLerp(0f, fullEffectDepth, waterState.HeadDepth);
        }

        underwaterVolume.weight = Mathf.MoveTowards(underwaterVolume.weight, targetWeight, fadeSpeed * Time.unscaledDeltaTime);
    }
}
