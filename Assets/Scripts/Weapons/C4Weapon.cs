using System.Collections.Generic;
using UnityEngine;

public class C4Weapon : ExplosiveWeapon
{
    private List<ExplosivesController> activedC4 = new List<ExplosivesController>();
    private bool c4sPlaced;
    
    public void HandleC4Action()
    {
        if (c4sPlaced)
            ExplodeAll();
        else
            DeployExplosive();
        
    }

    public void ExplodeAll()
    {
        foreach (ExplosivesController c4 in activedC4)
        {
            c4?.Explode();
        }
        activedC4.Clear();
        c4sPlaced = false;
    }

    protected override void OnEnable()
    {       
        base.OnEnable();
        c4sPlaced = false;
    }

    public override bool DeployExplosive()
    {
        if (!TryGetDeployPoint(out RaycastHit2D hit))
            return false;

        int positionX = Mathf.FloorToInt(firePoint.position.x);
        int positionY = Mathf.RoundToInt(hit.point.y);
        ExplosivesController c4 = Instantiate(explosivePrefab, new Vector2(positionX, positionY), Quaternion.identity).GetComponent<ExplosivesController>();
        c4.Initialize(explosionRadius, explosionDelay, damage);
        AudioManager.instance.PlayDeployWeaponSound();
        activedC4.Add(c4);
        c4sPlaced = true;
        return true;
    }
}
