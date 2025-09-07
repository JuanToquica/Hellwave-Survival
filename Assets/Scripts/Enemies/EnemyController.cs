using Pathfinding;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform firepoint;
    private Animator animator;
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
    [SerializeField] private float stopDistance;
    [SerializeField] private float knockbackForce;
    private bool dying;
    
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        InvokeRepeating("UpdatePath", 0f, 0.1f);
        dying = false;
    }


    void FixedUpdate()
    {
        LookAtPlayer();
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if ((distanceToPlayer > stopDistance || Physics2D.Raycast(firepoint.position, (player.position - firepoint.position), stopDistance, 1 << 3)) && !dying)
            CalculateMovementAndRepulsion();
        else
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, new Vector2(0, 0), smoothFactor);
        
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
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
                Vector2 closestPoint = enemy.ClosestPoint(transform.position);
                float dist = Vector2.Distance(transform.position, closestPoint);
                Vector2 dir = (Vector2)(transform.position - (Vector3)closestPoint);              
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

    private void LookAtPlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        if (directionToPlayer.normalized.x >= 0)
            transform.rotation = Quaternion.Euler(0, 180, transform.rotation.eulerAngles.z);
        else
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
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

    public void ApplyKnockback(Vector2 direction)
    {
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }

    public void OnDie()
    {
        dying = true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}
