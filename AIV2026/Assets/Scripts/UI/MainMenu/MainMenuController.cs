using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;

    public void OnPlayButtonPressed(string sceneToLoad)
    {
        sceneLoader.Load(sceneToLoad);
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
