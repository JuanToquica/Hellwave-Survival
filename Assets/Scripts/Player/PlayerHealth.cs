using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private PlayerController controller;
    public float health;

    private void Start()
    {
        health = maxHealth;
        controller = GetComponent<PlayerController>();
    }

    public void TakeDamage(float damage, Vector2 direction)
    {
        health -= damage;
        if (health < 0)
            Die();
        else
        {
            controller.ApplyKnockback(direction);
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }

    private void Die()
    {
        Debug.Log("PlayerMuerto");
    }
}
