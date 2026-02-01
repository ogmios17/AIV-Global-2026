using TMPro;
using UnityEngine;

public class TextSetter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    public void Awake()
    {
        GlobalData.Instance.text = this;
    }

    public void SetTextMessage(string message)
    {
        Debug.Log("Setting text to " + message);
        text.text = message;
    }
}
