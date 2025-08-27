using UnityEngine;

public class ShotgunBullet : BulletControler
{
    [HideInInspector] public int range;

    protected override void Update()
    {       
        if (travelledDistance > range) Destroy(gameObject);
        base.Update();
    }
}
