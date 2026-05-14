using UnityEngine;

public class BossPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float speed = 2f;

    public Animator bossAnimator;

    private Transform target;

    public bool isFighting = false;

    private Vector3 lastPosition;

    void Start()
    {
        target = pointB;

        
    }

    

    void Update()
    {
        // Stop everything during fight
        if (isFighting)
        {
            bossAnimator.SetBool("IsWalking", false);
            return;
        }

        // Move boss
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Rotate toward target
        // Rotate toward target smoothly
        Vector3 direction = (target.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                5f * Time.deltaTime
            );
        }

        // Detect REAL movement
        float movedDistance = Vector3.Distance(transform.position, lastPosition);

        if (movedDistance > 0.001f)
        {
            bossAnimator.SetBool("IsWalking", true);
        }
        else
        {
            bossAnimator.SetBool("IsWalking", false);
        }

        lastPosition = transform.position;

        // Switch patrol points
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            if (target == pointA)
            {
                target = pointB;
            }
            else
            {
                target = pointA;
            }
        }
    }
}