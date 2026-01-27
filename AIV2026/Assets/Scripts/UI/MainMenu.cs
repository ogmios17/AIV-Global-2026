using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayButtonPressed()
    {
        // TODO
        Debug.Log("Play button pressed - Load the game scene here.");
    }

    public void OnQuitButtonPressed()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
