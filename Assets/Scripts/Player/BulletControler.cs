using UnityEngine;

public class BulletControler : MonoBehaviour
{
    protected float speed;
    protected int damageAmount;
    protected Vector3 currentPosition;
    protected Vector3 direction;
    protected float travelledDistance;
    protected float distanceThisFrame;

    public virtual void Initialize(Vector3 startPos, Vector3 dir, float bulletSpeed, int damage)
    {
        currentPosition = startPos;
        direction = dir;
        speed = bulletSpeed;
        damageAmount = damage;
        travelledDistance = 0f;

        transform.position = currentPosition;
        transform.right = direction;
    }

    protected virtual void Update()
    {
        distanceThisFrame = speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, distanceThisFrame);
        if (hit.collider != null && !hit.transform.CompareTag("Player"))
        {
            if (hit.transform.CompareTag("Barrel"))
            {
                ExplosivesController barrel = hit.transform.GetComponent<ExplosivesController>();
                barrel.Explode();
            }
            Debug.Log("IMPACTO"); 
            Destroy(gameObject);
        }
        else
        {
            currentPosition += direction * distanceThisFrame;
            travelledDistance += distanceThisFrame;
            transform.position = currentPosition;
        }
    }

    protected void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
