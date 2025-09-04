using UnityEngine;

public class EnemyMeeleAttack : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private float distanceToAttackPlayer;
    [SerializeField] private Transform player;
    private Animator animator;
    private float nextAttack;
    private float distanceToPlayer;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= distanceToAttackPlayer && Time.time >= nextAttack) Attack();
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        
        nextAttack = Time.time + cooldown;
    }
}
