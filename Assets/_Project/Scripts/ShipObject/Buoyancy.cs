using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Buoyancy : MonoBehaviour
{
    [SerializeField] private float buoyancy;
    [SerializeField] private Transform floatCenter;
    [SerializeField] private float waterDamping;


    private WaterVolume contactedWater;
    private Collider myCollider;
    private Rigidbody rb;


    private void Awake()
    {
        if (!floatCenter)
        {
            floatCenter = transform;
        }

        myCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<WaterVolume>(out var water))
        {
            contactedWater = water;
            rb.linearDamping = waterDamping;
            rb.angularDamping = waterDamping / 10; // I should lowkey store the old val
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == contactedWater.gameObject)
        {
            contactedWater = null;
            rb.linearDamping = 0;
            rb.angularDamping = 0.05f;
        }
    }

    private void Update()
    {
        if (!contactedWater) return;

        var distanceToSurface = contactedWater.GetSurface().position.y - floatCenter.position.y;
        if (distanceToSurface < 0) return; // Above floatCenter

        var upForce = distanceToSurface * buoyancy * Time.deltaTime;

        rb.AddForce(new Vector3(0, upForce, 0));
    }
}
