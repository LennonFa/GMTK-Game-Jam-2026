using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SurvivorOxygen : MonoBehaviour
{
    public bool Drowned { get; private set; }

    [SerializeField] private float maxHoldBreathSeconds = 3;
    [SerializeField] private float submergeHeight = 0;
    private Vector3 SubmergePosition => transform.position + new Vector3(0, submergeHeight, 0);

    private float currentHoldBreathSeconds;
    private Collider myCollider;
    private bool waterborne;
    private bool submerged;
    private WaterVolume contactedWaterVolume;

    #region Editorstuff

    private void OnDrawGizmosSelected()
    {
        var submergeCenter = transform.position + new Vector3(0, submergeHeight, 0);
        Gizmos.DrawSphere(submergeCenter, 0.1f);
    }

    #endregion

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<WaterVolume>(out contactedWaterVolume))
        {
            Debug.Log("Hit Water");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<WaterVolume>(out var water))
        {
            Debug.Log("Left Water");
            contactedWaterVolume = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (contactedWaterVolume)
        {
            submerged = SubmergePosition.y < contactedWaterVolume.GetSurface().position.y;
            waterborne = true;
        }

        if (submerged)
            currentHoldBreathSeconds += Time.deltaTime;
        else if (currentHoldBreathSeconds > 0)
            currentHoldBreathSeconds -= Time.deltaTime;

        CheckDrowned();
    }

    private void CheckDrowned()
    {
        if (currentHoldBreathSeconds > maxHoldBreathSeconds)
        {
            Drown();
        }
    }

    void Drown()
    {
        Debug.Log($"{name} drowned!");
        Destroy(gameObject);
    }
}
