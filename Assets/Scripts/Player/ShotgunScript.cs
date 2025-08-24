using UnityEngine;

public class ShotgunScript : Weapon
{
    [SerializeField] private int spreadAngle;
    [SerializeField] private int amountOfBullets;


    public override void Fire()
    {
        muzleFlashAnimator.SetTrigger("Shot");
        for (int i = 0; i < amountOfBullets; i++)
        {
            BulletControler bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<BulletControler>();

            float angle = i == amountOfBullets/2? 0 : Random.Range(-spreadAngle, spreadAngle);
            Vector3 direction = player.localScale.x == 1 ? Quaternion.Euler(0, 0, angle) * firePoint.right : Quaternion.Euler(0, 0, angle) * -firePoint.right;
            bullet.Initialize(firePoint.position, direction, projectileSpeed, damage);
        }    
    }
}
