using System.Threading;
using UnityEngine;

public class GranadeController : MonoBehaviour
{
    [SerializeField] private float fuseTime;
    [SerializeField] private Rigidbody2D rb;
    private float timer;

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
        rb.AddForce(direction * force);
    }

    private void Explode()
    {
        Debug.Log("GRANADA HA EXPLOTADO");
        Destroy(gameObject);
    }
}
