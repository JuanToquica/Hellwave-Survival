using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static event Action<int> OnEnemiesKilledChanged;

    [Header("UI")]
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Gameplay")]
    public int currentRound;
    public bool isTheGamePaused;    

    [Header("Wave Management")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private float timeBetweenRounds;
    [SerializeField] private int numberOfSpawnersRoundOne;
    [SerializeField] private int numberOfEnemiesRoundOne;
    [SerializeField] private int limitOfEnemiesOnSceneRoundOne;
    [SerializeField] private int maxNumerOfSpawners;   
    [SerializeField] private int additionOfEnemiesPerRound;
    [SerializeField] private int additionOfLimitOfEnemiesOnScene;
    [SerializeField] private int maxLimitOfEnemiesOnScene;
    public int activedSpawners;
    public int enemiesToSpawn;
    public int limitOfEnemiesOnScene;
    public bool isRoundActive;


    public int aliveEnemies;
    public int deadEnemiesThisRound;
    public int deadEnemies;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        deadEnemiesThisRound = 0;
        currentRound = 1;
        activedSpawners = numberOfSpawnersRoundOne;
        enemiesToSpawn = numberOfEnemiesRoundOne;
        limitOfEnemiesOnScene = limitOfEnemiesOnSceneRoundOne;
        StartRound();
    }

    private void StartRound()
    {
        isRoundActive = true;
        enemySpawner.SpawnWave(limitOfEnemiesOnScene, activedSpawners, false);
    }

    private void EndRound()
    {
        isRoundActive = false;
        deadEnemiesThisRound = 0;
        currentRound++;
        if (activedSpawners < maxNumerOfSpawners)
        {
            if (currentRound == 2)
                activedSpawners+=3;
            else
                activedSpawners++;
        }
        enemiesToSpawn += additionOfEnemiesPerRound;
        if (limitOfEnemiesOnScene < maxLimitOfEnemiesOnScene)
        {
            limitOfEnemiesOnScene += additionOfLimitOfEnemiesOnScene;
            if (limitOfEnemiesOnScene > maxLimitOfEnemiesOnScene)
                limitOfEnemiesOnScene = maxLimitOfEnemiesOnScene;
        }
        Invoke("StartRound", timeBetweenRounds);
    }

    private void Update()
    {
        if (!isRoundActive) return;
        int remainingEnemiesToSpawn = enemiesToSpawn - (aliveEnemies + deadEnemiesThisRound);
        if (remainingEnemiesToSpawn == 0 && (deadEnemiesThisRound >= enemiesToSpawn))
        {
            EndRound();
        }
        else if (!enemySpawner.spawningWave && remainingEnemiesToSpawn > 0 && aliveEnemies < limitOfEnemiesOnScene)
        {
            int availableSpace = limitOfEnemiesOnScene - aliveEnemies;
            int enemiesAmount = remainingEnemiesToSpawn <= availableSpace ? remainingEnemiesToSpawn : availableSpace;
            enemySpawner.SpawnWave(enemiesAmount, activedSpawners, true);
        }     
    }

    public int GetNumberOfActivedSpawners()
    {
        return activedSpawners;
    }

    public void AddAliveEnemies(int amount)
    {
        aliveEnemies += amount;
    }

    public void OnEnemyDead()
    {
        deadEnemies++;
        deadEnemiesThisRound ++;
        aliveEnemies--;
        OnEnemiesKilledChanged?.Invoke(deadEnemies);
    }

    public void OnPauseClicked(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            PauseAndUnpauseGame();
        }
    }

    public void PauseAndUnpauseGame()
    {
        if (isTheGamePaused)
        {
            Time.timeScale = 1;
            pauseUI.SetActive(false);
            isTheGamePaused = false;
            InputManager.instance.EnablePlayerInputs();
        }
        else
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
            isTheGamePaused = true;
            InputManager.instance.DisablePlayerInputs();
        }
        AudioManager.instance.PlayButtonSound();        
    }
}
