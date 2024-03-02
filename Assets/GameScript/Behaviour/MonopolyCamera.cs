using UnityEngine;

public class MonopolyCamera : MonoBehaviour
{
    public Transform target; // The game object to follow
    public float distance = 5f; // Distance from the target
    public float smoothSpeed = 0.1f; // Smoothing factor for camera movement

    private Vector3 offset = new Vector3(-5.77f, 11.17f, -4.31f); // Initial offset between camera and target

    private void Start()
    {
        // Calculate the initial offset
        //if (target != null)
        //    offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position for the camera
            Vector3 desiredPosition = target.position + offset;

            // Use Mathf.Lerp to smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Set the camera position
            transform.position = smoothedPosition;

            // Make the camera look at the target
            //transform.LookAt(target);
        }
    }
}
