using UnityEngine;

public class BulletControler : MonoBehaviour
{
    private float speed;
    private int damageAmount;
    private Vector3 currentPosition;
    private Vector3 direction;
    private float travelledDistance;
    private float distanceThisFrame;

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

    private void Update()
    {
        distanceThisFrame = speed * Time.deltaTime;

        if (Physics2D.Raycast(currentPosition, direction, distanceThisFrame))
        {
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

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
