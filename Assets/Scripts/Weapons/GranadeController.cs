using System.Threading;
using UnityEngine;

public class GrenadeController : ExplosivesController
{
    [SerializeField] private Rigidbody2D rb;
    private float timer = 0;

    private void Update()
    {
        if (timer >= explosionDelay)
        {
            rb.simulated = false;
            Explode();
            this.enabled = false;
        }
        else
            timer += Time.deltaTime;
    }

    public void Launch(Vector2 direction,float force, Vector2 playerVelocity)
    {
        rb.linearVelocity = playerVelocity;
        timer = 0;
        Vector2 dir = direction.normalized;
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
}
