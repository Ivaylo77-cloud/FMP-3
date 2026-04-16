using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;

    [Header("Settings")]
    public float sensitivity = 100f;
    public float smoothTime = 0.05f;

    private float targetRotationX;
    private float currentRotationX;
    private float rotationVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;

        // Accumulate rotation (no deltaTime here)
        targetRotationX += mouseX * Time.deltaTime;

        // Smooth toward the target rotation
        currentRotationX = Mathf.SmoothDamp(
            currentRotationX,
            targetRotationX,
            ref rotationVelocity,
            smoothTime
        );

        playerBody.rotation = Quaternion.Euler(0f, currentRotationX, 0f);
    }
}