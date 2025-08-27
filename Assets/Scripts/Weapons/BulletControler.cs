using UnityEngine;

public class BulletControler : MonoBehaviour
{
    protected float speed;
    protected int damage;
    protected Vector3 currentPosition;
    protected Vector3 direction;
    protected float travelledDistance;
    protected float distanceThisFrame;

    public virtual void Initialize(Vector3 startPos, Vector3 dir, float bulletSpeed, int damage)
    {
        currentPosition = startPos;
        direction = dir;
        speed = bulletSpeed;
        this.damage = damage;
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
                barrel.Explode(false);
            }
            else if (hit.transform.CompareTag("Enemy"))
            {
                EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
                enemy.TakeDamage(damage);
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
