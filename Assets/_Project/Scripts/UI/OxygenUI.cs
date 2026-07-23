using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
   [SerializeField] private PlayerOxygen playerOxygen;
   [SerializeField] private Slider oxygenSlider;

   private void Awake()
    {
        oxygenSlider.minValue = 0f;
        oxygenSlider.maxValue = 1f;
    }

    private void Update()
    {
        oxygenSlider.value = playerOxygen.OxygenNormalized;
    }
}
