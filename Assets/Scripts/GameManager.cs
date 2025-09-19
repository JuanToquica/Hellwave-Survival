using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static event Action<int> OnEnemiesKilledChanged;
    public static event Action<string> OnRoundStarted;

    [Header("UI")]
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Gameplay")]
    public int currentRound;
    public bool isTheGamePaused;    

    [Header("Wave Management")]
    [SerializeField] private GameConfig gameData;
    [SerializeField] private EnemySpawner enemySpawner;
    private float timeBetweenRounds;
    private int numberOfSpawnersRoundOne;
    private int numberOfEnemiesRoundOne;
    private int limitOfEnemiesOnSceneRoundOne;
    private int additionOfEnemiesPerRound;
    private int additionOfLimitOfEnemiesOnScene;
    private int maxLimitOfEnemiesOnScene;
    private int highScore;
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
        timeBetweenRounds = gameData.timeBetweenRounds;
        numberOfSpawnersRoundOne = gameData.numberOfSpawnersRoundOne;
        numberOfEnemiesRoundOne = gameData.numberOfEnemiesRoundOne;
        limitOfEnemiesOnSceneRoundOne = gameData.limitOfEnemiesOnSceneRoundOne;
        additionOfEnemiesPerRound = gameData.additionOfEnemiesPerRound;
        additionOfLimitOfEnemiesOnScene = gameData.additionOfLimitOfEnemiesOnScene;
        maxLimitOfEnemiesOnScene = gameData.maxLimitOfEnemiesOnScene;        
    }

    private void Start()
    {
        AudioManager.instance.PlayMusic();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        InputManager.instance.EnablePlayerInputs();
        hud.SetActive(true);
        deadEnemies = 0;
        OnEnemiesKilledChanged?.Invoke(deadEnemies);
        deadEnemiesThisRound = 0;
        currentRound = 1;
        activedSpawners = numberOfSpawnersRoundOne;
        enemiesToSpawn = numberOfEnemiesRoundOne;
        limitOfEnemiesOnScene = limitOfEnemiesOnSceneRoundOne;
        Invoke("StartRound", timeBetweenRounds);
    }

    private void StartRound()
    {
        isRoundActive = true;
        OnRoundStarted?.Invoke($"Round {currentRound}");
        enemySpawner.SpawnWave(limitOfEnemiesOnScene, activedSpawners, false);
    }

    private void EndRound()
    {
        isRoundActive = false;
        deadEnemiesThisRound = 0;
        currentRound++;
        if (activedSpawners < enemySpawner.spawners.Length)
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

    public int GetScore()
    {
        return deadEnemies;
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
            hud.SetActive(true);
            isTheGamePaused = false;
            InputManager.instance.EnablePlayerInputs();
        }
        else
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
            hud.SetActive(false);
            isTheGamePaused = true;
            InputManager.instance.DisablePlayerInputs();
        }
        AudioManager.instance.PlayButtonSound();        
    }

    public void OnPlayerDead()
    {
        StartCoroutine(OnPlayerDeadCorutine());
    }

    private IEnumerator OnPlayerDeadCorutine()
    {
        isRoundActive = false;
        InputManager.instance.DisablePlayerInputs();
        yield return new WaitForSeconds(4);
        hud.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public bool TrySetNewScore(int score)
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    private void OnEnable() => PlayerHealth.OnPlayerDeath += OnPlayerDead;
    private void OnDisable() => PlayerHealth.OnPlayerDeath -= OnPlayerDead;
}
