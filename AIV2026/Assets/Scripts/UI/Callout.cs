using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Callout : MonoBehaviour
{
    [SerializeField] private Sprite sony;
    [SerializeField] private Sprite xbox;
    [SerializeField] private Sprite nintendo;
    [SerializeField] private Sprite keyboard;

    [SerializeField] private Image p1Sprite;
    [SerializeField] private Image p2Sprite;

    void Awake()
    {
        UpdateP1Sprite();
        UpdateP2Sprite();
        GlobalData.Instance.onP1ControllerChosen += UpdateP1Sprite;
        GlobalData.Instance.onP2ControllerChosen += UpdateP2Sprite;
    }

    void OnDestroy()
    {
        if (GlobalData.Instance == null) return;
        GlobalData.Instance.onP1ControllerChosen -= UpdateP1Sprite;
        GlobalData.Instance.onP2ControllerChosen -= UpdateP2Sprite;
    }

    public void UpdateP1Sprite()
    {
        UpdateSprite(p1Sprite, GlobalData.Instance.Player1?.Controller);
    }

    public void UpdateP2Sprite()
    {
        UpdateSprite(p2Sprite, GlobalData.Instance.Player2?.Controller);
    }

    private void UpdateSprite(Image target, InputDevice controller)
    {
        // CPU (o giocatore non ancora connesso): mostra la tastiera come fallback.
        if (controller == null)
        {
            target.sprite = keyboard;
            return;
        }

        string name = controller.description.product?.ToLower() ?? "";
        if (name.Contains("xbox"))
            target.sprite = xbox;
        else if (name.Contains("dualsense") || name.Contains("dualshock") || name.Contains("playstation"))
            target.sprite = sony;
        else if (name.Contains("switch") || name.Contains("pro"))
            target.sprite = nintendo;
        else
            target.sprite = keyboard;
    }
}
