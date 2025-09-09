using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject enemyMeele;
    [SerializeField] private GameObject enemyShooter;
    [SerializeField] private Transform[] spawners;
    [SerializeField] private float timeBetweenSpawns;
    public bool spawningWave;


    public void SpawnEnemies(int spawnersAmount)
    {
        for (int i = 0; i < spawnersAmount; i++)
        {
            GameObject enemy = Instantiate(enemyMeele, spawners[i].position, Quaternion.identity);
            EnemyMeeleAttack enemyAttack = enemy.GetComponent<EnemyMeeleAttack>();
            enemyAttack.SetPlayer(player);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.SetPlayer(player);
        }
        GameManager.instance.AddAliveEnemies(spawnersAmount);
    }

    public void SpawnWave(int numberOfEnemies, int spawnersAmount)
    {
        Debug.Log("Spawneando oleada" + numberOfEnemies + spawnersAmount);
        StartCoroutine(SpawnWaveCoroutine(numberOfEnemies, spawnersAmount));
    }

    private IEnumerator SpawnWaveCoroutine(int numberOfEnemies, int spawnersAmount)
    {
        spawningWave = true;
        int remainingEnemiesToSpawn = numberOfEnemies;

        while (true)
        {
            if (remainingEnemiesToSpawn >= spawnersAmount)
            {
                SpawnEnemies(spawnersAmount);
                remainingEnemiesToSpawn -= spawnersAmount;
            }
            else
            {
                SpawnEnemies(remainingEnemiesToSpawn);
                remainingEnemiesToSpawn -= remainingEnemiesToSpawn;
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
            if (remainingEnemiesToSpawn <= 0) break;
        }
        spawningWave = false;
    }
}
