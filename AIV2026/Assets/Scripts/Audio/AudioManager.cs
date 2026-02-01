using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private Bus masterBus;

    private const string VolumeKey = "MasterVolume";

    [Header("Move Cards")]
    [SerializeField] private MoveCard attack;
    [SerializeField] private MoveCard block;
    [SerializeField] private MoveCard grapple;
    [SerializeField] private MoveCard shove;

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
    [SerializeField] private EventReference PauseSong;

    private EventInstance lastHPInstance;
    private EventInstance crowdLoop;
    private EventInstance mainTitleInstance;
    private EventInstance crowdInstance;
    //private EventInstance CombatMusicInstance;

    private bool hasStartedMainTitle = false;

    private Coroutine crowdNomixRoutine;

    public EventInstance CombatMusicInstance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        masterBus = RuntimeManager.GetBus("bus:/");
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("HasSetInitialVolume"))
        {
            
            SetMasterVolume(0.5f);

            PlayerPrefs.SetInt("HasSetInitialVolume", 1);
            PlayerPrefs.Save();
        }
        else
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
            SetMasterVolume(savedVolume);
        }

        if (!hasStartedMainTitle)
        {
            PlayMainTitle();
            hasStartedMainTitle = true;
        }
    }
    #region MUSIC
    public void SetMasterVolume(float volume)
    {
        if (masterBus.isValid())
        {
            masterBus.setVolume(volume);
            PlayerPrefs.SetFloat(VolumeKey, volume);
            PlayerPrefs.Save();
        }
    }
    public float GetMasterVolume()
    {
        if (masterBus.isValid())
        {
            masterBus.getVolume(out float volume);
            return volume;
        }
        return 1f;
    }
    public void PlayLastHP()
    {
        RuntimeManager.PlayOneShot(lastHP);
    }
    public void PlayRecoverHP()
    {
        RuntimeManager.PlayOneShot(HPRecovery);
    }
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
        CombatMusicInstance = RuntimeManager.CreateInstance("event:/Music/Combat MX");
        CombatMusicInstance.start();
    }

    private void StartRandomCrowdNomix()
    {
        if (crowdNomixRoutine != null) return;
        crowdNomixRoutine = StartCoroutine(RandomCrowdNoMixCoroutine());


    }

    private void StopRandomCrowdNoMix()
    {
        if (crowdNomixRoutine != null)
        {
            StopCoroutine(crowdNomixRoutine);
            crowdNomixRoutine = null;
        }
    }
    private IEnumerator RandomCrowdNoMixCoroutine()
    {
        while (true)
        {
            EventInstance crowdInstance = RuntimeManager.CreateInstance(crowdsNomix);

            crowdInstance.setVolume(1f);
            crowdInstance.start();

            StartCoroutine(FadeOutAndStop(crowdInstance, 7f));
            float waitTime = Random.Range(13f, 17f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void StartCrowdNomixDelayed(float delay = 20f)
    {
        StartCoroutine(CrowdNomixAfterDelay(delay));
    }

    private IEnumerator CrowdNomixAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartRandomCrowdNomix(); // Usa la Coroutine già presente
    }

    public void UpdateCombatMusicByHealth(float crackenHP, float notZillaHP)
    {
        float total = crackenHP + notZillaHP;

        if (total <= 0) return;

        float crackRatio = Mathf.Pow(crackenHP / total, 0.5f); // sqrt
        float notZillaRatio = Mathf.Pow(notZillaHP / total, 0.5f);


        CombatMusicInstance.setParameterByName("Crack-Ken Volume", crackRatio);
            CombatMusicInstance.setParameterByName("NotZilla Volume", notZillaRatio);

    }
    public void PlayCrowdPanic(float volume = 1f)
    {
        EventInstance panicInstance = RuntimeManager.CreateInstance(crowdPanic);
        panicInstance.setVolume(volume);
        panicInstance.start();

        StartCoroutine(FadeOutAndStop(panicInstance, 4f));
    }
    public void CheckLastHP(float p1HP, float p2HP)
    {

        bool isLastHP = p1HP <= 1f || p2HP <= 1f;

        if (isLastHP && !lastHPInstance.isValid())
        {
            lastHPInstance = RuntimeManager.CreateInstance(lastHP);
            lastHPInstance.start();
        }
        else if (!isLastHP && lastHPInstance.isValid())
        {
            lastHPInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            lastHPInstance.release();
        }
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

    public void StartCrowdLoop(float fadeIn = 1f)
    {
        if (crowdLoop.isValid()) return;

        crowdLoop = RuntimeManager.CreateInstance(crowds);
        crowdLoop.setVolume(0f);
        crowdLoop.start();

        StartCoroutine(FadeVolume(crowdLoop, 0f, 1f, fadeIn));
    }

    public void StopCrowdLoop(float fadeOut = 1f)
    {
        if (!crowdLoop.isValid()) return;

        StartCoroutine(FadeOutAndStop(crowdLoop, fadeOut));
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

        PlayBellStart();
        yield return new WaitForSeconds(0.5f);
        // Crowd loop
        StartCrowdLoop();

        // Bell start
    }
    private void PlayPlayerIntro(Jammer player)
    {
        if (player.CharacterType == CharacterType.CrackKen)
            RuntimeManager.PlayOneShot(crackKenVerse);
        else
            RuntimeManager.PlayOneShot(notZillaVerse);

        if (player.PlayerType == PlayerType.Player1)
            PlayCrowdCheer(crowdCheersForP1);
        else
            PlayCrowdCheer(crowdCheersForP2);
    }

    public void PlayCrowdCheer(EventReference cheerEvent, float fadeInTime = 0.5f)
    {
        crowdInstance = RuntimeManager.CreateInstance(cheerEvent);
        crowdInstance.setVolume(0f);
        crowdInstance.start();

        StartCoroutine(FadeVolume(crowdInstance, 0f, 1f, fadeInTime));
    }
   
    public void PlayCardSound(MoveCard card)
    {
        if (card == null) return;

        if (card == attack)
            Play(attackCard);
        else if (card == block)
            Play(blockCard);
        else if (card == grapple)
            Play(grabCard);
        else if (card == shove)
            Play(pushCard);
    }
    private IEnumerator FadeVolume(EventInstance instance, float from, float to, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(from, to, t / time);
            instance.setVolume(v);
            yield return null;
        }
        instance.setVolume(to);
    }

    private IEnumerator FadeOutAndStop(EventInstance instance, float time)
    {
        float volume;
        instance.getVolume(out volume);

        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            instance.setVolume(Mathf.Lerp(volume, 0f, t / time));
            yield return null;
        }

        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }
    private void Play(EventReference evt)
    {
        FMODUnity.RuntimeManager.PlayOneShot(evt);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {

    }
}
