using Unity.VisualScripting;
using UnityEngine;

public class ThrownProjectile : MonoBehaviour
{
    public GameObject payload;
    private float force;
    private Vector3 velocity;
    private bool isStrong;
    [SerializeField] private float gravity;
    [SerializeField] private float rotationSpeed = 360;

    void Update()
    {
        velocity.y -= gravity * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;

        UpdateRotation();
    }

    public void UpdateRotation()
    {
        // two different "animations" based on strength for fun
        if (isStrong)
        {
            transform.rotation = Quaternion.AngleAxis(2 * rotationSpeed * Time.deltaTime, velocity) * transform.rotation;
        }
        else
        {
            var angles = transform.localRotation.eulerAngles;
            angles.y = angles.y + rotationSpeed * Time.deltaTime;
            var rotation = transform.localRotation;
            rotation.eulerAngles = angles;
            transform.localRotation = rotation;
        }

    }

    public void Throw(Vector3 direction, float throwForce, bool strong)
    {
        direction = direction.normalized;
        force = throwForce;
        velocity = force * direction;
        isStrong = strong;
        transform.rotation = Quaternion.LookRotation(velocity);
    }
}
