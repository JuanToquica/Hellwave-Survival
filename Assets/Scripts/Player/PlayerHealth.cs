using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float stunDuration;
    private PlayerController controller;
    private PlayerAttackManager playerAttack;
    public float health;

    private void Start()
    {
        health = maxHealth;
        controller = GetComponent<PlayerController>();
        playerAttack = GetComponent<PlayerAttackManager>();
    }

    public void TakeDamage(float damage, Vector2 direction)
    {
        health -= damage;
        if (health < 0)
            Die();
        else
        {
            controller.ApplyKnockback(direction, stunDuration);
            playerAttack.OnTakeDamage(stunDuration);
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
