using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class TextSetter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private TextMeshProUGUI countdown;

    public void Awake()
    {
        if(GlobalData.Instance.text == null)
            GlobalData.Instance.text = this;
    }

    public void SetTextMessage(string message)
    {
        text.text = message;
    }

    public void SetCountDownMessage(string message)
    {
        countdown.text = message;
    }
}
