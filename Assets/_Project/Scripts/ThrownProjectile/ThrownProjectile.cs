using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrownProjectile : MonoBehaviour
{
    public GameObject payload;
    [SerializeField] private float rotationSpeed = 360f;
    private Rigidbody rigidbodyComponent;
    //private float force;
    //private Vector3 velocity;
    //private bool isStrong;
    //[SerializeField] private float gravity;

    private void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    /*
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
    */

    public void Throw(Vector3 direction, float throwForce, bool strong)
    {
        rigidbodyComponent.AddForce(direction.normalized * throwForce, ForceMode.VelocityChange);

        Vector3 rotationAxis = strong ? transform.right : transform.up;

        float spinMultiplier = strong ? 2f : 1f;

        rigidbodyComponent.angularVelocity = 
            rotationAxis *
            rotationSpeed *
            spinMultiplier *
            Mathf.Deg2Rad;
    }

    public GameObject TakePayload()
    {
        if (payload == null)
            return null;

        GameObject takenPayload = payload;
        payload = null;

        takenPayload.transform.SetParent(null, true);

        return takenPayload;
    }
}
