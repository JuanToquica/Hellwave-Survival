using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    public static event System.Action OnPlayerDeath;
    [SerializeField] private float maxHealth;
    [SerializeField] private float stunDuration;
    [SerializeField] private float vignetteIntensity;
    [SerializeField] private float vignetteDuration;
    [SerializeField] private float decreaseFactor;
    private PlayerController controller;
    private PlayerAttackManager playerAttack;
    public Volume globalVolume;
    private Vignette vignette;
    public float health;
    private Coroutine damageCoroutine;

    private void Start()
    {
        health = maxHealth;
        controller = GetComponent<PlayerController>();
        playerAttack = GetComponent<PlayerAttackManager>();
        if (globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f;
        }
    }

    public void TakeDamage(float damage, Vector2 direction)
    {
        health -= damage;
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(TakeDamageEffect());

        
        if (health <= 0)
        {
            controller.ApplyKnockback(direction * 2, stunDuration);
            Die();
        }          
        else
        {
            controller.ApplyKnockback(direction, stunDuration);
            playerAttack.OnTakeDamage(stunDuration);
        }
    }

    public void Heal()
    {
        health = maxHealth;
    }

    private IEnumerator TakeDamageEffect()
    {
        vignette.intensity.value = vignetteIntensity;
        float duration = health <= 0 ? vignetteDuration * 5 : vignetteDuration;

        yield return new WaitForSeconds(duration);

        float intensity = vignette.intensity.value;
        while (vignette.intensity.value > 0)
        {
            intensity -= decreaseFactor;
            if (intensity < 0) intensity = 0;
            vignette.intensity.value = intensity;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        vignette.intensity.value = 0;
    }

    private void Die()
    {
        Debug.Log("PlayerMuerto");
        OnPlayerDeath?.Invoke();
    }
}
