using UnityEngine;

public class ShotgunBullet : BulletControler
{
    public int range;

    protected override void Update()
    {       
        if (travelledDistance > range) Destroy(gameObject);
        base.Update();
    }
}
