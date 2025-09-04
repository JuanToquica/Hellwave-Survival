using UnityEngine;

public class EnemyShooterAttack : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private float distanceToAttackPlayer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform player;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int damage;
    private Animator animator;
    private float nextAttack;
    private float distanceToPlayer;

    private void Start()
    {       
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= distanceToAttackPlayer && Time.time >= nextAttack) Attack();       
    }

    private void Attack()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, directionToPlayer, distanceToPlayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject != gameObject && !hit.transform.CompareTag("Player"))
                return;
        }
        animator.SetTrigger("Attack");       
        nextAttack = Time.time + cooldown;
    }

    public void LaunchProjectile()
    {
        BulletControler bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<BulletControler>();
        Vector3 direction = (player.position - transform.position).normalized;
        bullet.Initialize(firePoint.position, direction, projectileSpeed, damage, gameObject);
    }
}
