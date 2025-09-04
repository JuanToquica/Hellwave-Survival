using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private Animator animator;
    private float health;
    private EnemyController controller;
    private CircleCollider2D circleCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        health = maxHealth;
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
        Debug.Log("EnemigoMuerto");
        GameManager.instance.OnEnemyDead();
        animator.SetTrigger("Die");
        circleCollider.enabled = false;
        controller.OnDie();
        Invoke("Destroy",0.5f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
