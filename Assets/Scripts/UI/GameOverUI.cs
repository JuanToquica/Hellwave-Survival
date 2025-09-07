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
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void OnExitButtonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
