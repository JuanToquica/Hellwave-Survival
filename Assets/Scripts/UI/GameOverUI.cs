using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private Color scoreColor;
    [SerializeField] private Color highScoreColor;

    private void OnEnable()
    {
        if (GameManager.instance.TrySetNewScore(GameManager.instance.GetScore()))
        {
            scoreText.color = highScoreColor;
            scoreText.text = "NEW HIGH SCORE";
        }
        else
        {
            scoreText.color = scoreColor;
            scoreText.text = "SCORE";
        }

        scoreValue.text = GameManager.instance.GetScore().ToString();
    }

    private void Start()
    {
        retryButton.onClick.AddListener(OnRetryButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    public void OnRetryButtonClicked()
    {
        AudioManager.instance.PlayButtonSound();
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void OnExitButtonClicked()
    {
        AudioManager.instance.PlayButtonSound();
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
