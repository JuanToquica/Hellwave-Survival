using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private Animator animator;
    private float health;
    private EnemyController controller;
    [SerializeField] private CircleCollider2D circleCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
    }

    private void OnEnable()
    {
        health = maxHealth;
        circleCollider.enabled = true;
    }

    public void TakeDamage(float damage, Vector2 direction)
    {
        health -= damage;
        if (health < 0) 
            Die();
        else
        {
            animator.SetTrigger("TakeDamage");
            controller.ApplyKnockback(direction);
        }
            
    }

    private void Die()
    {
        GameManager.instance.OnEnemyDead();
        animator.SetTrigger("Die");
        circleCollider.enabled = false;
        controller.OnDie();
        Invoke("ReturnToPool", 0.5f);
    }

    private void ReturnToPool()
    {
        ObjectPoolManager.instance.ReturnPooledObject(gameObject);
    }
}
