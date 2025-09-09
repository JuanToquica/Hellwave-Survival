using UnityEngine;

public class ProjectileWeapon : WeaponBase
{
    [SerializeField] protected Animator muzleFlashAnimator;
    [SerializeField] protected GameObject bulletPrefab;
    protected int projectileSpeed;

    protected override void OnEnable()
    {
        projectileSpeed = weaponData.weapons[weaponIndex].speed;
        damage = weaponData.weapons[weaponIndex].damage;
    }

    public override void Fire()
    {
        muzleFlashAnimator.SetTrigger("Shot");
        BulletControler bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<BulletControler>();
        Vector3 direction = player.localScale.x == 1 ? firePoint.right : -firePoint.right;
        bullet.Initialize(firePoint.position, direction, projectileSpeed, damage, transform.root.gameObject);
        if (weaponIndex == 0) return; //Si es la primera arma, no disminuir municion, es infinita
        Ammo--;
        if (Ammo == 0)
        {
            playerAttackManager.OnOutOfAmmunition();
        }
    }
}
