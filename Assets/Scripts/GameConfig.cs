using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObject/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("General")]
    public float timeBetweenRounds;
    public int maxLimitOfEnemiesOnScene;

    [Header ("Round One Parameters")]
    public int numberOfSpawnersRoundOne;
    public int numberOfEnemiesRoundOne;
    public int limitOfEnemiesOnSceneRoundOne;
    
    [Header("Progression")]
    public int additionOfEnemiesPerRound;
    public int additionOfLimitOfEnemiesOnScene;   
}
