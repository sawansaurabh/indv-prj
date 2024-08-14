using UnityEngine;

public class RigidbodyTest : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from the GameObject.");
        }
        else
        {
            rb.isKinematic = false;  // Ensure it's not kinematic
            rb.AddForce(Vector3.forward * 10f);  // Apply a force to test
        }
    }

    void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;
        Vector3 angularVelocity = rb.angularVelocity;

        // Debug.Log($"Velocity: {velocity}");
        // Debug.Log($"Angular Velocity: {angularVelocity}");
    }
}
