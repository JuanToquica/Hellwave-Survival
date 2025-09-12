using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image jetpackBar;
    [SerializeField] private Image ammoIcon;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI ammoText;
    private WeaponBase currentWeapon;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthBar;
        PlayerController.OnFlyingTimerChanged += UpdateJetpackBar;
        GameManager.OnEnemiesKilledChanged += UpdateScore;
        PlayerAttackManager.OnWeaponChanged += OnWeaponChanged;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthBar;
        PlayerController.OnFlyingTimerChanged -= UpdateJetpackBar;
        GameManager.OnEnemiesKilledChanged -= UpdateScore;
        PlayerAttackManager.OnWeaponChanged -= OnWeaponChanged;
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
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
        ammoText.text = ammo.ToString();
    }
}
