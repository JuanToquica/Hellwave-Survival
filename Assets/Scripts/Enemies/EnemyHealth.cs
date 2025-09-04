using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private Animator animator;
    private float health;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0) 
            Die();
        else
            animator.SetTrigger("TakeDamage");
    }

    private void Die()
    {
        Debug.Log("EnemigoMuerto");
        GameManager.instance.OnEnemyDead();
        animator.SetTrigger("Die");
        Invoke("Destroy",0.5f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
