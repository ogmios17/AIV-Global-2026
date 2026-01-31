using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;


    [SerializeField] private EventReference mainTitleLoop;
    [SerializeField] private EventReference bellStart;
    [SerializeField] private EventReference grabCard;
    [SerializeField] private EventReference cancelCard;
    [SerializeField] private EventReference attackCard;
    [SerializeField] private EventReference blockCard;
    [SerializeField] private EventReference pushCard;
    [SerializeField] private EventReference radioActiveHit;
    [SerializeField] private EventReference crackKenVerse;
    [SerializeField] private EventReference crowds;
    [SerializeField] private EventReference crowdsNomix;
    [SerializeField] private EventReference crowdCheersForP1;
    [SerializeField] private EventReference crowdCheersForP2;
    [SerializeField] private EventReference crowdPanic;
    [SerializeField] private EventReference CombatMelody;
    [SerializeField] private EventReference helicopter;
    [SerializeField] private EventReference HPRecovery;
    [SerializeField] private EventReference lastHP;
    [SerializeField] private EventReference notZillaVerse;
    [SerializeField] private EventReference spamButtonPlayer1;
    [SerializeField] private EventReference spamButtonPlayer2;
    [SerializeField] private EventReference UIClickSelection;
    [SerializeField] private EventReference UIErrorClick;
    [SerializeField] private EventReference UIHover;
    [SerializeField] private EventReference UIPauseMenu;

    private EventInstance mainTitleInstance;
    private EventInstance crowdInstance;
    private EventInstance CombatMusicInstance;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #region MUSIC

    public void PlayMainTitle()
    {
        StopMainTitle();
        mainTitleInstance = RuntimeManager.CreateInstance(mainTitleLoop);
        mainTitleInstance.start();
    }

    public void StopMainTitle()
    {
        if (mainTitleInstance.isValid())
        {
            mainTitleInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            mainTitleInstance.release();
        }
    }

    public void PlayCombatMusic()
    {
        StopCombatMusic();
        CombatMusicInstance = RuntimeManager.CreateInstance(CombatMelody);
        CombatMusicInstance.start();
    }

    public void StopCombatMusic()
    {
        if (CombatMusicInstance.isValid())
        {
            CombatMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            CombatMusicInstance.release();
        }
    }
    #endregion
    public void PlayBellStart()
    {
        RuntimeManager.PlayOneShot(bellStart);
    }

    #region CARDS

    public void PlayAttackCard()
    {
        RuntimeManager.PlayOneShot(attackCard);
    }

    public void PlayBlockCard()
    {
        RuntimeManager.PlayOneShot(blockCard);
    }

    public void PlayGrabCard()
    {
        RuntimeManager.PlayOneShot(grabCard);
    }

    public void PlayPushCard()
    {
        RuntimeManager.PlayOneShot(pushCard);
    }

    public void PlayCancelCard()
    {
        RuntimeManager.PlayOneShot(cancelCard);
    }

    #endregion

    public void PlayRadioactiveHit()
    {
        RuntimeManager.PlayOneShot(radioActiveHit);
    }

    #region CROWD

    public void StartCrowd()
    {
        StopCrowd();
        crowdInstance = RuntimeManager.CreateInstance(crowds);
        crowdInstance.start();
    }

    public void StopCrowd()
    {
        if (crowdInstance.isValid())
        {
            crowdInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            crowdInstance.release();
        }
    }

    public void SetCrowdPanic(bool panic)
    {
        if (crowdInstance.isValid())
            crowdInstance.setParameterByName("Panic", panic ? 1 : 0);
    }

    public void CrowdCheerP1()
    {
        RuntimeManager.PlayOneShot(crowdCheersForP1);
    }

    public void CrowdCheerP2()
    {
        RuntimeManager.PlayOneShot(crowdCheersForP2);
    }

    #endregion

    #region UI

    public void PlayUIClick()
    {
        RuntimeManager.PlayOneShot(UIClickSelection);
    }

    public void PlayUIHover()
    {
        RuntimeManager.PlayOneShot(UIHover);
    }

    public void PlayUIError()
    {
        RuntimeManager.PlayOneShot(UIErrorClick);
    }

    public void PlayPauseMenu()
    {
        RuntimeManager.PlayOneShot(UIPauseMenu);
    }

    #endregion

    public void PlaySpamButtonP1()
    {
        RuntimeManager.PlayOneShot(spamButtonPlayer1);
    }

    public void PlaySpamButtonP2()
    {
        RuntimeManager.PlayOneShot(spamButtonPlayer2);
    }

    public void OnCombatSceneReady()
    {
        StartCoroutine(CombatIntroSequence());
    }
    private IEnumerator CombatIntroSequence()
    {
        // Player 1 intro
        PlayPlayerIntro(GlobalData.Instance.Player1);
        yield return new WaitForSeconds(2.0f);

        // Player 2 intro
        PlayPlayerIntro(GlobalData.Instance.Player2);
        yield return new WaitForSeconds(2.0f);

        // Crowd loop
        StartCrowd();

        // Bell start
        PlayBellStart();
    }
    private void PlayPlayerIntro(Jammer player)
    {
        if (player.CharacterType == CharacterType.CrackKen)
            RuntimeManager.PlayOneShot(crackKenVerse);
        else
            RuntimeManager.PlayOneShot(notZillaVerse);

        if (player.PlayerType == PlayerType.Player1)
            RuntimeManager.PlayOneShot(crowdCheersForP1);
        else
            RuntimeManager.PlayOneShot(crowdCheersForP2);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
}
