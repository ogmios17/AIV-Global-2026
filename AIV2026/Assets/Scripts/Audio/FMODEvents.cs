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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var vca in vcaList)
        {
            // Recupera l'istanza da FMOD
            vca.vcaInstance = FMODUnity.RuntimeManager.GetVCA(vca.vcaPath);

            // Imposta lo slider al valore attuale del VCA (per coerenza all'avvio)
            float currentVolume;
            vca.vcaInstance.getVolume(out currentVolume);
            if (vca.controllerSlider != null)
            {
                vca.controllerSlider.value = currentVolume;

                // Aggiunge l'ascoltatore via codice per non doverlo fare a mano in Unity
                vca.controllerSlider.onValueChanged.AddListener((val) => {
                    vca.vcaInstance.setVolume(val);
                });
            }
        }
    }

}
