using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject enemyMeele;
    [SerializeField] private GameObject enemyShooter;
    public Transform[] spawners;
    [Range(0, 1)]
    [SerializeField] private float probabilityOfEnemyShooterSpawn;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private float timeBetweenWaves;
    public bool spawningWave;


    public void SpawnEnemies(int spawnersAmount, bool randomSpawn)
    {
        int spawnedEnemies = 0;
        int spawn = 0;
        if (randomSpawn) spawn = Random.Range(0, GameManager.instance.GetNumberOfActivedSpawners());
            
        for (int i = 0; i < spawnersAmount; i++)
        {
            if (Physics2D.OverlapCircle(spawners[spawn].position, 0.7f, 1 << 8)) continue; //Comprobar que no haya acumulacion de enemigos en el spawn
            float random = Random.Range(0f, 1f);
            GameObject enemy;
            if (random < probabilityOfEnemyShooterSpawn)
            {               
                enemy = ObjectPoolManager.instance.GetPooledObject(enemyShooter, spawners[spawn].position, Quaternion.identity);
                EnemyShooterAttack enemyAttack = enemy.GetComponent<EnemyShooterAttack>();
                enemyAttack.SetPlayer(player);
            }
            else
            {
                enemy = ObjectPoolManager.instance.GetPooledObject(enemyMeele, spawners[spawn].position, Quaternion.identity);
                EnemyMeeleAttack enemyAttack = enemy.GetComponent<EnemyMeeleAttack>();
                enemyAttack.SetPlayer(player);
            } 
            
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.InitiateEnemy(player, spawners[spawn].right);
            spawn ++;
            spawnedEnemies++;
            if (spawn >= spawnersAmount) spawn = 0;
        }
        GameManager.instance.AddAliveEnemies(spawnedEnemies);
    }

    public void SpawnWave(int numberOfEnemies, int spawnersAmount, bool randomSpawn)
    {
        Debug.Log("Spawneando oleada" + numberOfEnemies + spawnersAmount);
        StartCoroutine(SpawnWaveCoroutine(numberOfEnemies, spawnersAmount, randomSpawn));
    }

    private IEnumerator SpawnWaveCoroutine(int numberOfEnemies, int spawnersAmount, bool randomSpawn)
    {
        spawningWave = true;
        int remainingEnemiesToSpawn = numberOfEnemies;

        while (true)
        {
            if (remainingEnemiesToSpawn >= spawnersAmount)
            {
                SpawnEnemies(spawnersAmount, randomSpawn);
                remainingEnemiesToSpawn -= spawnersAmount;
            }
            else
            {
                SpawnEnemies(remainingEnemiesToSpawn, randomSpawn);
                remainingEnemiesToSpawn -= remainingEnemiesToSpawn;
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
            if (remainingEnemiesToSpawn <= 0) break;
        }
        yield return new WaitForSeconds(timeBetweenWaves);
        spawningWave = false;
    }
}
