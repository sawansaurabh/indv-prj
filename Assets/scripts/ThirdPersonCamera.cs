using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player; // Reference to the player (or the object the camera follows)
    public float mouseSensitivity = 100f; // Sensitivity of the mouse
    public float distanceFromPlayer = 3f; // Distance of the camera from the player
    public Vector2 pitchLimits = new Vector2(-30, 60); // Limits for vertical rotation

    private float yaw = 0f; // Horizontal rotation
    private float pitch = 0f; // Vertical rotation

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust yaw and pitch based on mouse movement
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y); // Clamp the pitch

        // Rotate the camera around the player
        Vector3 direction = new Vector3(0, 0, -distanceFromPlayer);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = player.position + rotation * direction;

        // Always look at the player
        transform.LookAt(player.position + Vector3.up * 1.5f); // Adjust the look-at height as needed
    }
}
