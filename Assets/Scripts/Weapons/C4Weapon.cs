using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class C4Weapon : ExplosiveWeapon
{
    private List<ExplosivesController> activedC4 = new List<ExplosivesController>();
    public bool c4sPlaced;

    protected override void OnEnable()
    {
        PlayerAttackManager.OnWeaponChanged += OnWeaponChanged;
    }

    protected void OnDisable()
    {
        PlayerAttackManager.OnWeaponChanged -= OnWeaponChanged;
    }

    public void HandleC4Action()
    {
        if (c4sPlaced)
            StartCoroutine(ExplodeAll());
        else
            DeployExplosive();
        
    }

    public IEnumerator ExplodeAll()
    {
        for (int i = 0; i < activedC4.Count; i++)
        {
            var c4 = activedC4[i];
            c4?.Explode();
            yield return new WaitForSeconds(0.1f);
        }
        activedC4.Clear();
        c4sPlaced = false;
        if (Ammo == 0)
        {
            playerAttackManager.OnOutOfAmmunition();
        }
    }

    private void OnWeaponChanged(WeaponBase weapon)
    {
        if (weapon == this)
        {
            base.OnEnable();
            if (Ammo > 0)
                c4sPlaced = false;
        }       
    }

    public override bool DeployExplosive()
    {
        if (!TryGetDeployPoint(out RaycastHit2D hit))
            return false;

        int positionX = Mathf.FloorToInt(firePoint.position.x);
        int positionY = Mathf.RoundToInt(hit.point.y);
        ExplosivesController c4 = ObjectPoolManager.instance.GetPooledObject(explosivePrefab, new Vector2(positionX, positionY), Quaternion.identity).
            GetComponent<ExplosivesController>();
        c4.Initialize(explosionRadius, explosionDelay, damage, knockBackForce);
        AudioManager.instance.PlayDeployWeaponSound();
        activedC4.Add(c4);
        c4sPlaced = true;
        Ammo--;
        return true;
    }
}
