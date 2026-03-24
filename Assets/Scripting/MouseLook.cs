using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;   // Assign your player object

    [Header("Settings")]
    public float sensitivity = 100f;
    public float smoothTime = 0.05f;

    private float currentMouseX;
    private float currentVelocityX;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get raw mouse X input only
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;

        // Smooth the input
        currentMouseX = Mathf.SmoothDamp(currentMouseX, mouseX, ref currentVelocityX, smoothTime);

        // Rotate player left/right
        playerBody.Rotate(Vector3.up * currentMouseX);
    }
}