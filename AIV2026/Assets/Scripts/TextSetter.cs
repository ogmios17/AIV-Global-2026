using TMPro;
using UnityEngine;

public class TextSetter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private TextMeshProUGUI countdown;

    public void Awake()
    {
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
