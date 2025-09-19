using UnityEngine;

public class PoolInitializer : MonoBehaviour
{
    [SerializeField] private GameObject enemyMeelePrefab;
    [SerializeField] private GameObject enemyShooterPrefab;
    [SerializeField] private GameObject enemyProjectilePrefab;
    [SerializeField] private GameObject pistolBulletPrefab;
    [SerializeField] private GameObject shotgunBulletPrefab;
    [SerializeField] private GameObject carabineBulletPrefab;
    [SerializeField] private GameObject rifleBulletPrefab;
    [SerializeField] private GameObject rocketsBulletPrefab;
    [SerializeField] private GameObject barrelsPrefab;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private GameObject c4Prefab;
    [SerializeField] private GameObject ammunitionBoxPrefab;

    private void Start()
    {
        ObjectPoolManager.instance.CreatePool(enemyMeelePrefab,80);
        ObjectPoolManager.instance.CreatePool(enemyShooterPrefab, 20);
        ObjectPoolManager.instance.CreatePool(enemyProjectilePrefab, 10);
        ObjectPoolManager.instance.CreatePool(pistolBulletPrefab, 5);
        ObjectPoolManager.instance.CreatePool(shotgunBulletPrefab, 15);
        ObjectPoolManager.instance.CreatePool(carabineBulletPrefab, 5);
        ObjectPoolManager.instance.CreatePool(rifleBulletPrefab, 10);
        ObjectPoolManager.instance.CreatePool(rocketsBulletPrefab, 5);
        ObjectPoolManager.instance.CreatePool(barrelsPrefab, 30);
        ObjectPoolManager.instance.CreatePool(grenadePrefab, 4);
        ObjectPoolManager.instance.CreatePool(c4Prefab, 10);
        ObjectPoolManager.instance.CreatePool(ammunitionBoxPrefab, 10);
    }
}
