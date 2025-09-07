using UnityEngine;

public class EnemyMeeleAttack : EnemyAttackBase
{
    protected override void Attack()
    {
        animator.SetTrigger("Attack");
        
        nextAttack = Time.time + cooldown;
    }

    public void PerformAttack() //Se llamara desde un animation event
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionToPlayer, distanceToAttackPlayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject == gameObject) continue;
            if (hit.transform.CompareTag("Player"))
            {
                PlayerHealth player = hit.transform.GetComponent<PlayerHealth>();
                player.TakeDamage(damage, directionToPlayer);
                continue;
            }
            break;
        }
    }
}
