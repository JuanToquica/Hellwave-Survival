using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button exitButton;


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
