using UnityEngine;

public class LandMineController : ExplosivesController
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Invoke("Explode",explosionDelay);
        }
    }

    public override void Explode()
    {
        ApplyExplosionDamage();
    }
}
