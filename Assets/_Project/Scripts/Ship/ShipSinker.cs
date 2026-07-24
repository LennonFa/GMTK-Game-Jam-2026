using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ShipSinker : MonoBehaviour
{
    [SerializeField] private float mainSinkDistance;
    [SerializeField] private float timeToSink;
    [SerializeField] private Quaternion targetRotation;

    /// <summary>
    /// speed to uncapped sinking after main sink Lerp
    /// </summary>
    [SerializeField] private float slowSinkingSpeed;

    private float Progress => currentSinkTime / timeToSink;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 targetPosition;
    private float currentSinkTime;

    /// <summary>
    /// uncapped sinking for the end of the lerp
    /// </summary>
    private float slowSinkDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        targetPosition = transform.position;
        targetPosition.y -= mainSinkDistance;

    }

    // Update is called once per frame
    void Update()
    {
        currentSinkTime += Time.deltaTime;
        if (currentSinkTime > timeToSink)
        {
            currentSinkTime = timeToSink;
            slowSinkDistance += slowSinkingSpeed * Time.deltaTime;
        }

        UpdateTransform();
    }

    void UpdateTransform()
    {
        transform.position = Vector3.Lerp(startPosition, targetPosition, Progress);
        transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Progress);

        transform.position += new Vector3(0, -slowSinkDistance, 0);
    }
}
