using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persiste tra scene
        }
        else
        {
            Destroy(gameObject);  // Distrugge duplicati
        }
    }

    public void Load(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    public IEnumerator LoadAsync(string sceneName)
    {
        TransitionLayer transitionLayer = TransitionManager.Instance.TransitionLayer;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Transition: FADE-IN
        transitionLayer.Show(0.5f, 0.0f);

        while (asyncLoad.progress < 0.9f || !transitionLayer.isDone)
            yield return null;

        asyncLoad.allowSceneActivation = true;

        // Transition: FADE-OUT
        transitionLayer.Hide(0.5f, 1f);
    }
}
