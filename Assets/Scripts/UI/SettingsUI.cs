using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private Button backButton;
    [SerializeField] private Slider volume;
    [SerializeField] private Button resetControlsSettings;
    [SerializeField] private RebindManager rebindManager;

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
        mainMenuUI.SetActive(true);
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
        rebindManager.ResetActionMaps();
        EventSystem.current.SetSelectedGameObject(null);
    }


    private void OnEnable()
    {
        EventSystem.current.firstSelectedGameObject = volume.gameObject;
        LoadVolumeSettins();
    }
}

