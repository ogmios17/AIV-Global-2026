using FMOD.Studio;
using FMODUnity;
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
    [SerializeField] private EventReference defaultMELNomix;
    [SerializeField] private EventReference defaultMXNomix;
    [SerializeField] private EventReference helicopter;
    [SerializeField] private EventReference HPRecovery;
    [SerializeField] private EventReference notZillaVerse;
    [SerializeField] private EventReference radioactivePLayer;
    [SerializeField] private EventReference spamButtonPlayer1;
    [SerializeField] private EventReference spamButtonPlayer2;
    [SerializeField] private EventReference UIClickSelection;
    [SerializeField] private EventReference UIErrorClick;
    [SerializeField] private EventReference UIHover;
    [SerializeField] private EventReference UIPauseMenu;

    private EventInstance musicInstance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource masterSource;

    private float masterVolume = 1f;
    private bool isMuted = false;
    private const string PREF_MASTER = "MasterVolume";

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void StopActionMusic()
    {
        
    }
    public void TestButton()
    {
        RuntimeManager.PlayOneShot(crackKenVerse);
    }
    public void PlayCrackKenVerse(int variant)
    {
        var e = RuntimeManager.CreateInstance(crackKenVerse);
        e.setParameterByName("Variant", variant);
        e.start();
        e.release();
    }

    private void ApplyAudioSettings()
    {
        float muteFactor = isMuted ? 0f : 1f;

        if (masterSource) masterSource.volume = masterVolume * muteFactor;    
    }
    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetFloat(PREF_MASTER, value);
        ApplyAudioSettings();
    }
    public void InitializeUI(Slider master)
    {
        if (master != null)
        {
            master.value = masterVolume;
            master.onValueChanged.RemoveAllListeners();
            master.onValueChanged.AddListener(SetMasterVolume);
            SetMasterVolume(master.value);
        }
    }
    public void Clicksound()
    {
        RuntimeManager.PlayOneShot(UIClickSelection);
    }
    public void CrowdSound()
    {

    }
    public void NotZillaSound()
    {

    }
    public void CrackKenSound()
    {

    }
    public void DefaultMx()
    {

    }
    public void DefaultMelody()
    {

    }
    public void NotZillaLifeUp()
    {

    }
    public void CrackKenLifeUp()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
}
