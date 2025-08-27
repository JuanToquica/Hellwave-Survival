using System.Collections.Generic;
using UnityEngine;

public class C4Weapon : Weapon
{
    public List<ExplosivesController> activedC4;
    public bool c4sPlaced;
    public void ExplodeAll()
    {
        foreach (ExplosivesController c4 in activedC4)
        {
            c4.Explode();
        }
        activedC4.Clear();
        c4sPlaced = false;
    }

    protected override void OnEnable()
    {
        c4sPlaced = false;
    }

    public override void Deploy(RaycastHit2D hit)
    {
        int positionX = Mathf.FloorToInt(firePoint.position.x);
        int positionY = Mathf.RoundToInt(hit.point.y);
        ExplosivesController c4 = Instantiate(bulletPrefab, new Vector2(positionX, positionY), Quaternion.identity).GetComponent<ExplosivesController>();
        activedC4.Add(c4);
        c4sPlaced = true;
    }
}
