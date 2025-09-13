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

    private void OnEnable() => PlayerHealth.OnTargetChanged += SetPlayer;
    private void OnDisable() => PlayerHealth.OnTargetChanged -= SetPlayer;

    protected void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
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
