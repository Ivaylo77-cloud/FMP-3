using UnityEngine;

public class NPC : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform player;

    public float speed = 3f;
    public float chaseSpeed = 5f;
    public float detectionRange = 8f;

    private Transform targetPoint;
    private bool isChasing = false;

    void Start()
    {
        targetPoint = pointA;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Detect player
        if (distanceToPlayer < detectionRange)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }
}