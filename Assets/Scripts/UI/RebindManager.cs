using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RebindManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Awake()
    {
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
}
