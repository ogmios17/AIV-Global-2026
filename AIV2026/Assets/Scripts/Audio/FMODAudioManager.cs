using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODAudioManager Instance { get; private set; }

    private void Awake()
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

    //metodo da chiamare per SFX semplici OneShot
    public void PlaySimpleSFX(EventReference evt)
    {
        if (evt.IsNull) return;

        RuntimeManager.PlayOneShot(evt);
    }

    //Metodo da chiamare per suoni in loop come musica o ambience, oppure per suoni che hanno parametri. N.B. L'instanza in loop va stoppata da StopAudioInstance()
    public EventInstance PlayAudioInstance(EventReference evt, params (string name, float value)[] parameters)
    {
        if (evt.IsNull)
        {
            return default;
        }

        EventInstance instance = RuntimeManager.CreateInstance(evt);

        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                instance.setParameterByName(param.name, param.value);
            }
        }

        instance.start();
        instance.release(); //se è un'instanza diversa da un loop, distrugge l'instanza a suono concluso

        return instance;
    }

    //Metodo per stoppare instanze in loop
    public void StopAudioInstance(EventInstance evtInstance)
    {
        var instance = evtInstance;
        evtInstance = default;

        if (instance.isValid())
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
