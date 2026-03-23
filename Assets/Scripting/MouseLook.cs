using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;   // Assign the player (for horizontal rotation)

    [Header("Settings")]
    public float sensitivity = 100f;
    public float smoothTime = 0.05f;
    public float minY = -90f;
    public float maxY = 90f;

    float xRotation = 0f;

    private float currentMouseX;
    private float currentMouseY;
    private float currentVelocityX;
    private float currentVelocityY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get raw mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        // Smooth the input
        currentMouseX = Mathf.SmoothDamp(currentMouseX, mouseX, ref currentVelocityX, smoothTime);
        currentMouseY = Mathf.SmoothDamp(currentMouseY, mouseY, ref currentVelocityY, smoothTime);

        // Vertical rotation (camera)
        xRotation -= currentMouseY;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (player body)
        playerBody.Rotate(Vector3.up * currentMouseX);
    }
}