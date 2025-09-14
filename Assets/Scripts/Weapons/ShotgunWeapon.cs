using UnityEngine;

public class ShotgunWeapon : ProjectileWeapon
{
    [SerializeField] private ShotgunData shotgunData;
    private int spreadAngle;
    private int amountOfBullets;

    protected override void OnEnable()
    {
        base.OnEnable();
        spreadAngle = shotgunData.spreadAngle;
        amountOfBullets = shotgunData.amountOfBullets;
    }

    public override void Fire()
    {
        muzleFlashAnimator.SetTrigger("Shot");
        for (int i = 0; i < amountOfBullets; i++)
        {
            ShotgunBullet bullet = ObjectPoolManager.instance.GetPooledObject(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<ShotgunBullet>();

            float angle = i == amountOfBullets/2? 0 : Random.Range(-spreadAngle, spreadAngle);
            Vector3 direction = player.localScale.x == 1 ? Quaternion.Euler(0, 0, angle) * firePoint.right : Quaternion.Euler(0, 0, angle) * -firePoint.right;
            bullet.Initialize(firePoint.position, direction, projectileSpeed,projectileRange, damage, knockBackForce, transform.root.gameObject);
        }
        Ammo--;
        if (Ammo == 0)
        {
            playerAttackManager.OnOutOfAmmunition();
        }
    }
}
