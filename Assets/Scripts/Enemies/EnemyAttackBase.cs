using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackBase : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    [SerializeField] protected float distanceToAttackPlayer;
    [SerializeField] protected Transform player;
    [SerializeField] protected int damage;
    protected Animator animator;
    protected float nextAttack;
    protected float distanceToPlayer;   

    protected void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= distanceToAttackPlayer && Time.time >= nextAttack) Attack();
    }

    protected virtual void Attack()
    {
        return;
    }
}
