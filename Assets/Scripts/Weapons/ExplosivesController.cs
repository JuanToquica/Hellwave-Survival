using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngineInternal;

public class ExplosivesController : MonoBehaviour
{
    [HideInInspector] public bool isExploding = false;
    protected float explosionRadius;
    protected float explosionDelay;    
    protected int damage;

    public virtual void Initialize(float explosionRadius,float explosionDelay,int damage)
    {
        this.explosionRadius = explosionRadius;
        this.explosionDelay = explosionDelay;
        this.damage = damage;
    }

    public virtual void Explode()
    {
        ApplyExplosionDamage();
    }

    public virtual void Explode(bool triggeredByExplosion)
    {
        if (isExploding) return;
        isExploding = true;
        if (triggeredByExplosion) Invoke("ApplyExplosionDamage", explosionDelay);
        else ApplyExplosionDamage();
    }

    protected void ApplyExplosionDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + transform.right * 0.5f, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            Vector2 direction = (hit.transform.position - transform.position).normalized;
            if (hit.transform.CompareTag("Barrel"))
            {
                ExplosivesController barrel = hit.GetComponent<ExplosivesController>();
                if (barrel != null && !barrel.isExploding)
                    barrel.Explode(true);
            }
            else if (hit.transform.CompareTag("Enemy"))
            {
                EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();                
                enemy.TakeDamage(damage, direction);
            }
            else if (hit.transform.CompareTag("Player"))
            {
                PlayerHealth player = hit.transform.GetComponent<PlayerHealth>();
                player.TakeDamage(damage, direction);
            }
        }
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + transform.right * 0.5f, explosionRadius); 
    }
}
