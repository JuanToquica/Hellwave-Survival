using UnityEngine;

public class EnemyShooterAttack : EnemyAttackBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed;
    

    protected override void Attack()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, directionToPlayer, distanceToPlayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject != gameObject && !hit.transform.CompareTag("Player") && !hit.transform.CompareTag("Barrel"))
                return;
        }
        animator.SetTrigger("Attack");       
        nextAttack = Time.time + cooldown;
    }

    public void LaunchProjectile() //Se llamara desde un animation event
    {
        BulletControler bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<BulletControler>();
        Vector3 direction = (player.position - transform.position).normalized;
        bullet.Initialize(firePoint.position, direction, projectileSpeed, damage, gameObject);
    }
}
