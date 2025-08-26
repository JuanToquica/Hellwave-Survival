using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    [SerializeField] protected Transform player;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] protected Animator muzleFlashAnimator;
    [SerializeField] protected int weaponIndex;
    public Transform rightGripPoint;
    public Transform leftGripPoint;
    protected int projectileSpeed;
    protected int damage;

    protected virtual void OnEnable()
    {
        projectileSpeed = weaponData.weapons[weaponIndex].speed;
        damage = weaponData.weapons[weaponIndex].damage;
    }

    public virtual void Fire() 
    {
        muzleFlashAnimator.SetTrigger("Shot"); 
        BulletControler bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<BulletControler>();
        Vector3 direction = player.localScale.x == 1 ? firePoint.right : -firePoint.right;
        bullet.Initialize(firePoint.position, direction, projectileSpeed, damage);
    }
    public virtual void Fire(float force, Vector2 playerVelocity)
    {

    }

    public void Deploy(RaycastHit2D hit)
    {
        int positionX = Mathf.FloorToInt(firePoint.position.x);
        int positionY = Mathf.RoundToInt(hit.point.y);
        Instantiate(bulletPrefab, new Vector2(positionX, positionY), Quaternion.identity);
    }
    
}
