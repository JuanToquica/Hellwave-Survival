using System.Threading;
using UnityEngine;

public class GranadeController : MonoBehaviour
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

    public void Launch(Vector2 direction,float force)
    {
        timer = 0;
        Vector2 dir = direction.normalized;
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }

    private void Explode()
    {
        Debug.Log("GRANADA HA EXPLOTADO");
        Destroy(gameObject);
    }
}
