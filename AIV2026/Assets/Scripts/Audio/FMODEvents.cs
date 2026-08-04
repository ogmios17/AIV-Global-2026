using FMOD.Studio;
using FMODUnity;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance;

    [System.Serializable]
    public class VCAElement
    {
        public string name;
        public string vcaPath;
        public Slider controllerSlider;

        [HideInInspector]
        public FMOD.Studio.VCA vcaInstance;
    }

    public List<VCAElement> vcaList;

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

    private void Start()
    {
        foreach (var vca in vcaList)
        {
            // 1. Recupera l'istanza del VCA da FMOD
            vca.vcaInstance = RuntimeManager.GetVCA(vca.vcaPath);

            // 2. Genera una chiave univoca di salvataggio per ogni VCA (es. "VCA_Volume_Music")
            string saveKey = "VCA_Volume_" + vca.name;

            // 3. Legge il valore salvato su disco. Se non esiste, imposta di default 0.5f (50%)
            float savedVolume = PlayerPrefs.GetFloat(saveKey, 0.5f);

            // 4. Applica subito il volume caricato all'istanza FMOD
            vca.vcaInstance.setVolume(savedVolume);

            // 5. Configura lo slider visivo (se presente)
            if (vca.controllerSlider != null)
            {
                // Rimuove vecchi listener per evitare duplicati
                vca.controllerSlider.onValueChanged.RemoveAllListeners();

                // Imposta la posizione visiva dello slider al valore salvato
                vca.controllerSlider.value = savedVolume;

                // Quando muovi lo slider: aggiorna FMOD e salva su disco
                vca.controllerSlider.onValueChanged.AddListener((val) =>
                {
                    vca.vcaInstance.setVolume(val);
                    PlayerPrefs.SetFloat(saveKey, val);
                    PlayerPrefs.Save();
                });
            }
        }
    }

}
