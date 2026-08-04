using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("Music Settings")]
    [Tooltip("Assegna le tracce musicali")]
    [SerializeField] private EventReference musicTitle;
    [SerializeField] private EventReference musicCombat;
    private EventInstance musicTitleInstance;
    private EventInstance musicCombatInstance;


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

    private void Start()
    {
        musicTitleInstance = FMODAudioManager.Instance.PlayAudioInstance(musicTitle);
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
        // chiamata ridondante, la gestiamo da handle scene music
      // if (IsCombatScene(sceneName))
      // {
      //     musicCombatInstance = FMODAudioManager.Instance.PlayAudioInstance(musicCombat);
      // }

        asyncLoad.allowSceneActivation = true;

        HandleSceneMusic(sceneName);

        // Transition: FADE-OUT
        transitionLayer.Hide(0.5f, 1f);


        yield return new WaitUntil(() => transitionLayer.isDone);

        if (IsCombatScene(sceneName))
        {
            AudioManager.Instance.OnCombatSceneReady();

            AudioManager.Instance.StartCrowdNomixDelayed(20f);
        }
    }
    private bool IsCombatScene(string sceneName)
    {
        return sceneName == "SampleScene";
    }
    private void HandleSceneMusic(string sceneName)
    {
        if (IsCombatScene(sceneName))
        {
            if (musicTitleInstance.isValid())
            {
                FMODAudioManager.Instance.StopAudioInstance(musicTitleInstance);
            }

            musicCombatInstance = FMODAudioManager.Instance.PlayAudioInstance(musicCombat);
        }
        else
            return;
     //{
     //    if (musicTitleInstance.isValid())
     //    { 
     //        FMODAudioManager.Instance.StopAudioInstance(musicCombatInstance); 
     //    }
     //
     //    musicTitleInstance = FMODAudioManager.Instance.PlayAudioInstance(musicTitle);
     //}
    }
}
