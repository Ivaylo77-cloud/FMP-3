using UnityEngine;

public class BossPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float speed = 2f;

    public Animator bossAnimator;

    private Transform target;

    void Start()
    {
        target = pointB;

        // Start walking animation
        bossAnimator.SetBool("IsWalking", true);
    }

    void Update()
    {
        // Move boss
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Rotate toward target
        Vector3 direction = (target.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }

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