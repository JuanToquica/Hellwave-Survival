using UnityEngine;

public class ExplosiveWeapon : WeaponBase
{
    [SerializeField] protected ExplosivesData explosiveData;
    [SerializeField] protected GameObject explosivePrefab;
    protected float explosionRadius;
    protected float explosionDelay;

    protected override void OnEnable()
    {
        explosionRadius = explosiveData.explosionRadius;
        explosionDelay = explosiveData.explosionDelay;
        damage = weaponData.weapons[weaponIndex].damage;
    }

    public bool TryGetDeployPoint(out RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(firePoint.position, -player.transform.up, 3.3f);
        if (hit.collider == null) return false;
        if (!Physics2D.Raycast(player.position + player.right * 0.5f * player.localScale.x, player.right * player.localScale.x, 1) && hit.transform.CompareTag("Ground"))
        {
            return true;
        }
        hit = default;
        return false;
    }

    public override bool DeployExplosive()
    {
        if (!TryGetDeployPoint(out RaycastHit2D hit))
            return false;

        int positionX = Mathf.FloorToInt(firePoint.position.x);
        int positionY = Mathf.RoundToInt(hit.point.y);
        ExplosivesController explosive = Instantiate(explosivePrefab, new Vector2(positionX, positionY), Quaternion.identity).GetComponent<ExplosivesController>();
        explosive.Initialize(explosionRadius, explosionDelay, damage);
        return true;
    }
}
