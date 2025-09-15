using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image jetpackBar;
    [SerializeField] private Image ammoIcon;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private GameObject floatingMessagePrefab;
    [SerializeField] private Transform roundMessageContainer;
    [SerializeField] private Transform messageContainer;
    [SerializeField] private float roundMessageSize;
    [SerializeField] private float messageSize;
    [SerializeField] private float timeBetweenMessages;
    private WeaponBase currentWeapon;
    private Queue<string> messages = new Queue<string>();
    private bool isShowing;


    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthBar;
        PlayerHealth.OnPlayerDeath += SetToCeroHealthBar;
        PlayerHealth.OnPlayerHealed += EnqueueMessage;
        PlayerController.OnFlyingTimerChanged += UpdateJetpackBar;
        GameManager.OnEnemiesKilledChanged += UpdateScore;
        GameManager.OnRoundStarted += EnqueueMessage;
        PlayerAttackManager.OnWeaponChanged += OnWeaponChanged;
        PlayerAttackManager.OnWeaponUnlocked += EnqueueMessage;
        PlayerAttackManager.OnCollectedAmmo += EnqueueMessage;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthBar;
        PlayerHealth.OnPlayerDeath -= SetToCeroHealthBar;
        PlayerHealth.OnPlayerHealed -= EnqueueMessage;
        PlayerController.OnFlyingTimerChanged -= UpdateJetpackBar;
        GameManager.OnEnemiesKilledChanged -= UpdateScore;
        GameManager.OnRoundStarted -= EnqueueMessage;
        PlayerAttackManager.OnWeaponChanged -= OnWeaponChanged;
        PlayerAttackManager.OnWeaponUnlocked -= EnqueueMessage;
        PlayerAttackManager.OnCollectedAmmo -= EnqueueMessage;
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0.015f, 1);
    }

    private void SetToCeroHealthBar()
    {
        healthBar.fillAmount = 0;
    }


    private void UpdateJetpackBar(float currentFuel, float maxFuel)
    {
        jetpackBar.fillAmount = currentFuel / maxFuel;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    private void OnWeaponChanged(WeaponBase newWeapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnAmmoChanged -= UpdateAmmo;
        }

        currentWeapon = newWeapon;
        if (currentWeapon != null)
        {
            currentWeapon.OnAmmoChanged += UpdateAmmo;
            ammoIcon.sprite = currentWeapon.ammoIcon;
            UpdateAmmo(currentWeapon.Ammo, currentWeapon.MaxAmmo);
        }
    }

    private void UpdateAmmo(int ammo, int maxAmmo)
    {
        if (currentWeapon.weaponIndex == 0)
            ammoText.text = "\u221E";
        else
            ammoText.text = ammo.ToString() + "/" + maxAmmo.ToString();
    }

    public void EnqueueMessage(string text, bool roundMessage)
    {
        messages.Enqueue(text);

        if (!isShowing)
            StartCoroutine(ShowMessages(roundMessage));
    }

    private IEnumerator ShowMessages(bool roundMessage)
    {
        isShowing = true;

        while (messages.Count > 0)
        {
            string msg = messages.Dequeue();
            if (roundMessage)
                SpawnMessage(msg, roundMessageContainer, roundMessageSize);
            else
                SpawnMessage(msg, messageContainer, messageSize);

            yield return new WaitForSeconds(timeBetweenMessages);
        }

        isShowing = false;
    }

    public void SpawnMessage(string message, Transform container, float size)
    {
        GameObject instance = Instantiate(floatingMessagePrefab, container);
        instance.GetComponent<FloatingMessage>().Show(message, size);
    }
}
