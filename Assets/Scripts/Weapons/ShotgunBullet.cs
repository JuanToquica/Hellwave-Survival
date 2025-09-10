using UnityEngine;

public class ShotgunBullet : BulletControler
{
    protected override void Update()
    {       
        if (travelledDistance > range) 
            ObjectPoolManager.instance.ReturnPooledObject(gameObject);
        base.Update();
    }
}
