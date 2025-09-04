using UnityEngine;

public class BulletControler : MonoBehaviour
{
    protected float speed;
    protected int damage;
    protected Vector3 currentPosition;
    protected Vector3 direction;
    protected float travelledDistance;
    protected float distanceThisFrame;
    protected GameObject launcher;

    public virtual void Initialize(Vector3 startPos, Vector3 dir, float bulletSpeed, int damage, GameObject launcher)
    {
        currentPosition = startPos;
        direction = dir;
        speed = bulletSpeed;
        this.damage = damage;
        travelledDistance = 0f;
        this.launcher = launcher;

        transform.position = currentPosition;
        transform.right = direction;
    }

    protected virtual void Update()
    {
        distanceThisFrame = speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, distanceThisFrame);
        if (hit.collider != null && hit.transform.gameObject != launcher)
        {
            transform.position = hit.point;
            if (hit.transform.CompareTag("Barrel"))
            {
                ExplosivesController barrel = hit.transform.GetComponent<ExplosivesController>();
                barrel.Explode(false);
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
