using UnityEngine;
//using UnityEngine.InputSystem;

public class MonopolyCamera : MonoBehaviour
{
    enum CameraState
    {
        Following = 1,
        FreeMove = 2,
    }
    CameraState state = CameraState.FreeMove;
    public void SetCameraFollowing()
    {
        this.state = CameraState.Following;
    }
    public void SetCameraFreeMove()
    {
        this.state = CameraState.FreeMove;
    }
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
        if (target != null && this.state == CameraState.Following)
        {
            UpdateFollowing();
        }
        else if (this.state == CameraState.FreeMove)
        {
            UpdateFreeMove();
        }
    }
    void UpdateFollowing()
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
    public float dragSpeed = 0.1f; // Adjust this value to control the camera movement speed

    private Vector2 touchPos; // Stores the starting touch position
    private bool isDragging; // Flag to indicate if dragging is happening

    void UpdateFreeMove()
    {
        //if (Mouse.current.leftButton.isPressed)
        //{
        //    if (Mouse.current.leftButton.wasPressedThisFrame) // Check for initial click
        //    {
        //        touchPos = Mouse.current.position.ReadValue();
        //    }
        //    else // Handle dragging
        //    {
        //        Vector3 mouseDelta = Mouse.current.position.ReadValue() - touchPos;
        //        transform.Translate(mouseDelta.x * Time.deltaTime, mouseDelta.y * Time.deltaTime, 0);
        //    }
        //}

        if (Input.touchCount != 1) // Only handle single touch
        {
            return;
        }
        
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            // Start dragging on touch begin
            isDragging = true;
            touchPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved && isDragging)
        {
            // Move camera on touch drag
            Vector3 delta = touch.position - touchPos;
            transform.position += new Vector3(-delta.x, 0f, -delta.y) * Camera.main.transform.position.z / Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            touchPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            // Stop dragging on touch end or cancel
            isDragging = false;
        }
    }
}
