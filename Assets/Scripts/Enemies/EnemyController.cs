using Pathfinding;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform player;
    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D rb;

    [Header ("Movement and Repulsion")]
    [SerializeField] private float speed;
    [SerializeField] private float nextWaypointDistance;
    [SerializeField] private float separationRadius;
    [SerializeField] private float separationForce;
    [SerializeField] private float smoothFactor;
    [SerializeField] private float distanceToAttackPlayer;

    
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.1f);
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= distanceToAttackPlayer)
            Attack();
        else
            CalculateMovementAndRepulsion();
    }

    private void CalculateMovementAndRepulsion()
    {
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count) return;

        Vector2 waypointDir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector2 velocity = waypointDir * speed;

        Vector2 separation = Vector2.zero;
        Collider2D[] otherEnemies = Physics2D.OverlapCircleAll(transform.position, separationRadius);
        foreach (var enemy in otherEnemies)
        {
            if (enemy != null && enemy.transform != transform && enemy.CompareTag("Enemy"))
            {
                Vector2 dir = (Vector2)(transform.position - enemy.transform.position);
                float dist = dir.magnitude;
                if (dist > 0)
                {
                    float repulsion = (separationRadius - dist) / separationRadius;
                    separation += dir.normalized * repulsion;
                }
            }
        }

        separation *= separationForce;

        Vector2 desiredVelocity = velocity + separation;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, desiredVelocity, smoothFactor);

        if (Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }


    private void UpdatePath()
    {
        if (seeker.IsDone() && player != null)
        {
            seeker.StartPath(rb.position, player.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 1;
        }
    }
    

    private void Attack()
    {
        Debug.Log("Atacando player");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}
