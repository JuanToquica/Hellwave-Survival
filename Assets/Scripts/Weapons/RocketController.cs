using UnityEngine;
using System.Collections;

public class RocketController : ExplosivesController
{
    protected float speed;
    protected int range;
    protected Vector3 currentPosition;
    protected Vector3 direction;
    protected float travelledDistance;
    protected float distanceThisFrame;
    protected GameObject launcher;
    [SerializeField] private TrailRenderer trail;
    private bool destroying;

    private void OnEnable()
    {
        destroying = false;
        trail.Clear();
    }

    public virtual void Initialize(Vector3 startPos, Vector3 dir, float bulletSpeed, int damage, GameObject launcher, float explosionRadius, float explosionDelay)
    {
        base.Initialize(explosionRadius, explosionDelay, damage);
        currentPosition = startPos;
        direction = dir;
        speed = bulletSpeed;
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
            animator.SetTrigger("Impact");
            AudioManager.instance.PlayExplosionSound();
            ApplyExplosionDamage();
        }
        else
        {
            currentPosition += direction * distanceThisFrame;
            travelledDistance += distanceThisFrame;
            transform.position = currentPosition;
        }
    }
}
