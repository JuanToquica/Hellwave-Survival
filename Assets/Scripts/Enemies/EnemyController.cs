using Pathfinding;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed;
    [SerializeField] private float stopDistanceThreshold;
    private AIPath path;
    private float distanceToTarget;

    private void Start()
    {
        path = GetComponent<AIPath>();
        
        
    }

    void Update()
    {
        path.maxSpeed = speed;
        distanceToTarget = Vector2.Distance(transform.position, player.position);
        if (distanceToTarget < stopDistanceThreshold)
        {
            path.destination = transform.position;
        }
        else
        {
            path.destination = player.position;
        }
    }
}
