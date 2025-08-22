using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform player;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private int weaponIndex;
    public Transform rightGripPoint;
    public Transform leftGripPoint;
    private int projectileSpeed;
    private int damage;

    private void OnEnable()
    {
        projectileSpeed = weaponData.weapons[weaponIndex].speed;
        damage = weaponData.weapons[weaponIndex].damage;
    }

    public void Fire() 
    {
        BulletControler bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<BulletControler>();
        Vector3 direction = player.localScale.x == 1 ? firePoint.right : -firePoint.right;
        bullet.Initialize(firePoint.position, direction, projectileSpeed, damage);
    }
}
