using UnityEngine;

public class InitialBarrel : ExplosivesController
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private ExplosivesData explosivesData;
    private void OnEnable()
    {
        this.explosionRadius = explosivesData.explosionRadius;
        this.explosionDelay = explosivesData.explosionDelay;
        this.damage = weaponData.weapons[4].damage;
        this.knockbackForce = weaponData.weapons[4].knockBackForce;
        isExploding = false;
    }
}
