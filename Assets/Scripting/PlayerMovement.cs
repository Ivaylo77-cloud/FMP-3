using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float moveSpeed = 6f;
    public float jumpForce = 7f;

    public float runSpeed = 10f;

    public int maxJumps = 2;

    public float gravity = -20f;
    public float glideGravity = -5f;

    private int jumpsLeft;

    //Stamina
    public float maxStamina = 5f;
    public float stamina;
    public float rechargeRate = 1f;
    public float sprintDrain = 1f;
    public float glideDrain = 0.8f;
    public float jumpCost = 1.5f;

    
    public Slider staminaBar;


    void Start()
    {
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        Move();
        Run();
        Jump();
        Glide();
        RechargeStamina();

        staminaBar.value = stamina / maxStamina;
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S

        Vector3 move = transform.right * x + transform.forward * z;

        rb.linearVelocity = new Vector3(
        move.x * moveSpeed,
        rb.linearVelocity.y,
        move.z * moveSpeed
);


        transform.position += move * moveSpeed * Time.deltaTime;
        
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            moveSpeed = runSpeed;
            stamina -= sprintDrain * Time.deltaTime;
        }
        else
        {
            moveSpeed = 6f;
           
        }
    }    

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0 && stamina >= jumpCost) 
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpsLeft --;
            stamina -= jumpCost;
        }
    }

    void Glide()
    {
        if (Input.GetKey(KeyCode.Space) && rb.linearVelocity.y < 0 && stamina > 0)
        {
            rb.linearVelocity += Vector3.up * glideGravity * Time.deltaTime;
            stamina -= glideDrain * Time.deltaTime;
        }
        else
        {
            rb.linearVelocity += Vector3.up * gravity * Time.deltaTime;
        }




    }

    void RechargeStamina()
    {
        if (stamina < maxStamina)
        {
            stamina += rechargeRate * Time.deltaTime;
        }
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            jumpsLeft = maxJumps;
        }
    }
}
