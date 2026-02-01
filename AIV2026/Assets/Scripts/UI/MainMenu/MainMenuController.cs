using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private StateManager stateManager;

    public void OnPlayButtonPressed(bool isCPUMode)
    {
        PlayerPrefs.SetInt("IsCPUMode", isCPUMode ? 1 : 0);
        SceneLoader.Instance.Load("CharacterSelection");
    }

    public void OnQuitButtonPressed()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
