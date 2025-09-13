using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class PlayerHealth : MonoBehaviour
{
    public static event System.Action OnPlayerDeath;
    public static event Action<float, float> OnHealthChanged;
    [SerializeField] private GameObject PlayerRagdollRightPrefab;
    [SerializeField] private GameObject PlayerRagdollLeftPrefab;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthPerKill;
    [SerializeField] private float healthRegenRate;
    [SerializeField] private float stunDuration;
    [SerializeField] private float vignetteIntensity;
    [SerializeField] private float vignetteDuration;
    [SerializeField] private float vignetteDecreaseFactor;
    private PlayerController controller;
    private PlayerAttackManager playerAttack;
    public Volume globalVolume;
    private Vignette vignette;
    private Coroutine damageCoroutine;
    private bool takingDamage;

    private float _health;
    public float health
    {
        get { return _health; }
        set
        {
            if (_health != value)
            {
                _health = value;
                OnHealthChanged?.Invoke(_health, maxHealth);
            }
        }
    }

    private void OnEnable()
    {
        GameManager.OnEnemiesKilledChanged += HealOnKill;
    }

    private void Start()
    {
        health = maxHealth;
        controller = GetComponent<PlayerController>();
        playerAttack = GetComponent<PlayerAttackManager>();
        takingDamage = false;
        if (globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f;
        }
    }

    private void Update()
    {
        if  (health < maxHealth && !takingDamage)
        {
            health = Mathf.Clamp(health += healthRegenRate * Time.deltaTime, 0f, maxHealth);
        }
    }

    public void TakeDamage(float damage, Vector2 direction)
    {
        health -= damage;
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(TakeDamageEffect());

        controller.ApplyKnockback(direction, stunDuration);
        if (health <= 0)
        {
            StartCoroutine(Die());
        }          
        else
        {
            playerAttack.OnTakeDamage(stunDuration);
        }
    }

    private void HealOnKill(int amount)
    {
        health = Mathf.Clamp(health += healthPerKill, 0f, maxHealth);
    }

    public void Heal()
    {
        health = maxHealth;
    }

    private IEnumerator TakeDamageEffect()
    {
        takingDamage = true;
        vignette.intensity.value = vignetteIntensity;
        float duration = health <= 0 ? vignetteDuration * 5 : vignetteDuration;

        yield return new WaitForSeconds(duration);

        float intensity = vignette.intensity.value;
        while (vignette.intensity.value > 0)
        {
            intensity -= vignetteDecreaseFactor;
            if (intensity < 0) intensity = 0;
            vignette.intensity.value = intensity;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        vignette.intensity.value = 0;
        takingDamage = false;
    }

    private void OnDisable()
    {
        GameManager.OnEnemiesKilledChanged -= HealOnKill;
    }

    private IEnumerator Die()
    {
        OnPlayerDeath?.Invoke();

        yield return new WaitForSeconds(0.05f);

        GameObject prefab = PlayerRagdollRightPrefab;
        if (transform.localScale.x < 0)
            prefab = PlayerRagdollLeftPrefab;

        GameObject ragdollInstance = Instantiate(prefab, transform.position, transform.rotation);
        cameraController.player = ragdollInstance.transform.Find("Body").GetComponent<Transform>();
        
        Rigidbody2D[] ragdollBodies = ragdollInstance.GetComponentsInChildren<Rigidbody2D>();
        foreach (var rb in ragdollBodies)
        {
            rb.linearVelocity = controller.GetLinearVelocity();
        }
        Collider2D[] colliders = ragdollInstance.GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = i + 1; j < colliders.Length; j++)
            {
                Physics2D.IgnoreCollision(colliders[i], colliders[j], true);
            }
        }

        gameObject.SetActive(false);       
    }
}
