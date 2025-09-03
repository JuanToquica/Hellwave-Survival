using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float health;

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0) Die();
    }

    private void Die()
    {
        Debug.Log("EnemigoMuerto");
        GameManager.instance.OnEnemyDead();
        Destroy(gameObject);
    }
}
