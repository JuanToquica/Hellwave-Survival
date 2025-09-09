using UnityEngine;
using System.Collections;
using UnityEditor.Search;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject enemyMeele;
    [SerializeField] private GameObject enemyShooter;
    [SerializeField] private Transform[] spawners;
    [Range(0, 1)]
    [SerializeField] private float probabilityOfEnemyShooterSpawn;
    [SerializeField] private float timeBetweenSpawns;
    public bool spawningWave;


    public void SpawnEnemies(int spawnersAmount, bool randomSpawn)
    {
        int spawn = 0;
        if (randomSpawn) spawn = Random.Range(0, GameManager.instance.GetNumberOfActivedSpawners());
            
        for (int i = 0; i < spawnersAmount; i++)
        {
            float random = Random.Range(0f, 1f);
            GameObject enemy;
            if (random < probabilityOfEnemyShooterSpawn)
            {
                enemy = Instantiate(enemyShooter, spawners[spawn].position, Quaternion.identity);
                EnemyShooterAttack enemyAttack = enemy.GetComponent<EnemyShooterAttack>();
                enemyAttack.SetPlayer(player);
            }
            else
            {
                enemy = Instantiate(enemyMeele, spawners[spawn].position, Quaternion.identity);
                EnemyMeeleAttack enemyAttack = enemy.GetComponent<EnemyMeeleAttack>();
                enemyAttack.SetPlayer(player);
            } 
            
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.SetPlayer(player);
            spawn ++;
            if (spawn >= spawnersAmount - 1) spawn = 0;
        }
        GameManager.instance.AddAliveEnemies(spawnersAmount);
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
        yield return new WaitForSeconds(2);
        spawningWave = false;
    }
}
