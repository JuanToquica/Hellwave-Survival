using UnityEngine;
using System.Collections;

public class BulletControler : MonoBehaviour
{
    protected float speed;
    protected int damage;
    protected int range;
    protected Vector3 currentPosition;
    protected Vector3 direction;
    protected float travelledDistance;
    protected float distanceThisFrame;
    protected GameObject launcher;
    [SerializeField] private Animator animator;
    [SerializeField] private TrailRenderer trail;
    private bool destroying;

    private void OnEnable()
    {
        destroying = false;
        trail.Clear();
    }

    public virtual void Initialize(Vector3 startPos, Vector3 dir, float bulletSpeed,int range ,int damage, GameObject launcher)
    {
        currentPosition = startPos;
        direction = dir;
        speed = bulletSpeed;
        this.range = range;
        this.damage = damage;
        travelledDistance = 0f;
        this.launcher = launcher;

        transform.position = currentPosition;
        transform.right = direction;     
    }

    protected virtual void Update()
    {
        if (destroying) return;
        distanceThisFrame = speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, distanceThisFrame);
        if (hit.collider != null && hit.transform.gameObject != launcher)
        {
            transform.position = hit.point;
            destroying = true;
            if (transform.CompareTag("EnemyProjectile")) AudioManager.instance.PlayFireBallImpactSound();
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
            StartCoroutine(PlayAnimationAndReturnToPool());         
        }
        else
        {
            currentPosition += direction * distanceThisFrame;
            travelledDistance += distanceThisFrame;
            transform.position = currentPosition;

            if (travelledDistance >= range)
                ObjectPoolManager.instance.ReturnPooledObject(gameObject);
        }
    }

    private IEnumerator PlayAnimationAndReturnToPool()
    {
        animator.SetTrigger("Impact");
        yield return new WaitForSeconds(0.5f);
        ObjectPoolManager.instance.ReturnPooledObject(gameObject);
    }

}
