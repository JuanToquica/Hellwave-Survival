using System.Threading;
using UnityEngine;

public class GranadeController : ExplosivesController
{
    [SerializeField] private GrenadeData grenadeData;
    [SerializeField] private Rigidbody2D rb;
    private float timer;
    private float fuseTime;
    private float extraMultiplier;

    private void OnEnable()
    {
        fuseTime = grenadeData.fuseTime;
        extraMultiplier = grenadeData.extraMultiplier;
    }

    private void Update()
    {
        if (timer >= fuseTime)
        {
            Explode();
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
