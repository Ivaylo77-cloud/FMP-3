using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;

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

    [Header("Audio")]
    public AudioSource walkAudio;
    public AudioSource sprintAudio;
    public AudioSource jumpAudio;
    public AudioSource glideAudio;

    private Coroutine glideFadeCoroutine;
    public float glideFadeDuration = 0.5f;

    public Slider staminaBar;


    void Start()
    {
        jumpsLeft = maxJumps;

        animator.Play("Idle");
    }

    void Update()
    {
        Move();
        Run();
        Jump();
        Glide();
        RechargeStamina();

        staminaBar.value = stamina / maxStamina;

        UpdateAnimations();

        HandleAudio();

        
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

            jumpAudio.PlayOneShot(jumpAudio.clip);

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

    void UpdateAnimations()
    {
        //Horizontal movement speed
        Vector3 horizontalVelocity = new Vector3(
        rb.linearVelocity.x,
        0,
        rb.linearVelocity.z
    );

        float speed = horizontalVelocity.magnitude;

        // Check grounded
        bool isGrounded = jumpsLeft == maxJumps;

        // Check gliding
        bool isGliding = Input.GetKey(KeyCode.Space)
                         && rb.linearVelocity.y < 0
                         && stamina > 0;

        // Check running
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && speed > 0.1f;

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsGliding", isGliding);
        //animator.SetBool("IsRunning", isRunning);
    }

    void HandleAudio()
    {
        // Horizontal speed only
        Vector3 horizontalVelocity = new Vector3(
            rb.linearVelocity.x,
            0,
            rb.linearVelocity.z
        );

        float speed = horizontalVelocity.magnitude;

        bool isMoving = speed > 0.1f;
        bool isGrounded = jumpsLeft == maxJumps;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && stamina > 0;

        bool isGliding = Input.GetKey(KeyCode.Space)
                         && rb.linearVelocity.y < 0
                         && stamina > 0;

        // WALK
        if (isGrounded && isMoving && !isRunning)
        {
            if (!walkAudio.isPlaying)
                walkAudio.Play();
        }
        else
        {
            if (walkAudio.isPlaying)
                walkAudio.Stop();
        }

        // RUN
        if (isGrounded && isMoving && isRunning)
        {
            if (!sprintAudio.isPlaying)
                sprintAudio.Play();
        }
        else
        {
            if (sprintAudio.isPlaying)
                sprintAudio.Stop();
        }

        // GLIDE
        if (isGliding)
        {
            // Stop fade if gliding starts again
            if (glideFadeCoroutine != null)
            {
                StopCoroutine(glideFadeCoroutine);
                glideFadeCoroutine = null;
            }

            glideAudio.volume = 1f;

            if (!glideAudio.isPlaying)
                glideAudio.Play();
        }
        else
        {
            if (glideAudio.isPlaying && glideFadeCoroutine == null)
            {
                glideFadeCoroutine = StartCoroutine(FadeOutGlide());
            }
        }
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpsLeft = maxJumps;
        }
    }

    IEnumerator FadeOutGlide()
    {
        float startVolume = glideAudio.volume;

        while (glideAudio.volume > 0)
        {
            glideAudio.volume -= startVolume * Time.deltaTime / glideFadeDuration;
            yield return null;
        }

        glideAudio.Stop();

        // Reset volume for next glide
        glideAudio.volume = 1f;

        glideFadeCoroutine = null;
    }
}
