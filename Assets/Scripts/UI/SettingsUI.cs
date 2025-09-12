using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Slider volume;
    [SerializeField] private Button resetControlsSettings;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
        resetControlsSettings.onClick.AddListener(OnResetControlsClicked);
        LoadVolumeSettins();
    }

    private void LoadVolumeSettins()
    {
        volume.value = PlayerPrefs.GetFloat("Volume");
    }

    public void OnBackButtonClicked()
    {
        AudioManager.instance.PlayButtonSound();
        gameObject.SetActive(false);
    }

    public void OnVolumeChanged()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
    }

    [ContextMenu("Eliminar player prefbs")]
    public void EliminarKeys()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnResetControlsClicked()
    {
        AudioManager.instance.PlayButtonSound();
        InputManager.instance.ResetActionMaps();
        EventSystem.current.SetSelectedGameObject(null);
    }


    private void OnEnable()
    {
        LoadVolumeSettins();
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}

