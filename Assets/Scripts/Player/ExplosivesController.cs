using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ExplosivesController : MonoBehaviour
{
    public float explosionRadius;
    public float explosionDelay;
    [HideInInspector] public bool isExploding = false;

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
            if (hit.transform.CompareTag("Barrel"))
            {
                ExplosivesController barrel = hit.GetComponent<ExplosivesController>();
                if (barrel != null && !barrel.isExploding)
                    barrel.Explode(true);
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
