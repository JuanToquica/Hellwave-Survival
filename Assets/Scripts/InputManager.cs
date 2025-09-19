using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public PlayerInput playerInput;

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
        playerInput = GetComponent<PlayerInput>();
        LoadActionMaps();
    }

    private void LoadActionMaps()
    {
        foreach (var map in playerInput.actions.actionMaps)
        {
            string json = PlayerPrefs.GetString(map.name);
            if (!string.IsNullOrEmpty(json))
            {
                map.LoadBindingOverridesFromJson(json);
            }
        }
    }

    public void ResetActionMaps()
    {
        foreach (var map in playerInput.actions.actionMaps)
        {
            if (PlayerPrefs.HasKey(map.name))
            {
                PlayerPrefs.DeleteKey(map.name);
            }
            map.RemoveAllBindingOverrides();
        }
    }

    public void DisablePlayerInputs()
    {
        playerInput.actions.FindActionMap("Player").Disable();
    }

    public void EnablePlayerInputs()
    {
        playerInput.actions.FindActionMap("Player").Enable();
    }
}
