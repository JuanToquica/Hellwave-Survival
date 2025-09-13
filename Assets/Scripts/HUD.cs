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
        PlayerHealth.OnPlayerDeath += SetToCeroHealthBar;
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
}
