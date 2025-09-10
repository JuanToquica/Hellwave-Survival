using UnityEngine;

public class RocketsWeapon : ProjectileWeapon
{
    [SerializeField] private ExplosivesData rocketData;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Fire()
    {
        muzleFlashAnimator.SetTrigger("Shot");
        RocketController bullet = ObjectPoolManager.instance.GetPooledObject(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<RocketController>();
        Vector3 direction = player.localScale.x == 1 ? firePoint.right : -firePoint.right;
        bullet.Initialize(firePoint.position, direction, projectileSpeed, damage, transform.root.gameObject, rocketData.explosionRadius, rocketData.explosionDelay);
        Ammo--;
        if (Ammo == 0)
        {
            playerAttackManager.OnOutOfAmmunition();
        }
    }
}
