using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioExample : MonoBehaviour
{
    [Header("Audio Musica")]
    [Tooltip("Assegna l'evento musica di FMOD")] // suggerimento visibile nell'inspector quando passi con il mouse sopra musicaRef
    [SerializeField] private EventReference musicaRef; // campo visibile nell'inspector
    private EventInstance musicaInstance;


    private void OnEnable()
    {
        musicaInstance = RuntimeManager.CreateInstance(musicaRef); //crea un'istanza dell'evento musica di fmod

        musicaInstance.start(); // play dell'instanza

        Debug.Log("Play music");
    }

    private void OnDisable() // metodo chiamato quando l'oggetto viene disabilitato
    {
        StopMusic();
        Debug.Log("Stop music OnDisable");
    }

    private void OnDestroy() // metodo che viene eseguito quando l'oggetto sparisce dalla scena
    {
        StopMusic(); // ferma la musica
        Debug.Log("Stop music OnDestroy");
    }

    public void StopMusic()
    {
        if (musicaInstance.isValid())
        {
            musicaInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //comando per stoppare l'instanza in play
        }
    }
}
