using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public Transform followTarget;
    public float adjustSpeed;
    public float horizontalOffset = 0f; // Horizontal offset
    public float fixedYPosition = 0f; // Fixed Y position

    void Start()
    {
        fixedYPosition = transform.position.y; // Fix the Y position at the start
    }

    void Update()
    {
        // Smoothly interpolate the vertical movement
        float vertical = Mathf.Lerp(transform.position.y, followTarget.position.y, adjustSpeed * Time.deltaTime);
        
        // Apply the horizontal offset and keep the Y fixed
        float horizontal = followTarget.position.x + horizontalOffset; // Horizontal offset applied here
        float zPosition = followTarget.position.z; // Follow target's Z position

        // Set the camera's new position with a fixed Y position
        transform.position = new Vector3(horizontal, fixedYPosition, zPosition);
    }
}
