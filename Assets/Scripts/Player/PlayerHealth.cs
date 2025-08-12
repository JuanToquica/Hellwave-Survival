using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float health;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0) Die();
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }

    private void Die()
    {

    }
}
