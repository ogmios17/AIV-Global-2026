using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;

public class SoundUI : MonoBehaviour
{
    [Header("FMOD UI Events (Globali)")]
    [Tooltip("Suono quando ci si sposta tra i pulsanti con le freccette")]
    [SerializeField] private EventReference selectSound;

    [Tooltip("Suono quando si preme Invio / Spazio / Tasto Conferma")]
    [SerializeField] private EventReference submitSound;

    private void Start()
    {
        Button[] allButtons = GetComponentsInChildren<Button>(true);

        foreach (Button button in allButtons)
        {
            button.onClick.AddListener(() =>
            {
                FMODAudioManager.Instance.PlaySimpleSFX(submitSound);

            });

            // Recupera l'EventTrigger se esiste già sul pulsante, altrimenti lo aggiunge al volo
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = button.gameObject.AddComponent<EventTrigger>();

            // Crea il listener per la selezione (tastiera/freccette)
            EventTrigger.Entry selectEntry = new EventTrigger.Entry();
            selectEntry.eventID = EventTriggerType.Select;
            selectEntry.callback.AddListener((data) =>
            {
                FMODAudioManager.Instance.PlaySimpleSFX(selectSound);
            });

            trigger.triggers.Add(selectEntry);
        }

    }
}


