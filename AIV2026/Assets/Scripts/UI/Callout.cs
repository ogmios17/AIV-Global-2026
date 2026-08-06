using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
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
        GlobalData.Instance.onP1ControllerChosen -= UpdateP1Sprite;
        GlobalData.Instance.onP2ControllerChosen += UpdateP2Sprite;
    }

    public void UpdateP1Sprite()
    {
        InputDevice controller = GlobalData.Instance.Player1.Controller;
        string name = controller.description.product.ToLower();
        if (name.Contains("xbox"))
        {
            p1Sprite.sprite = xbox;
        }else if (name.Contains("dualsense") || name.Contains("dualshock") || name.Contains("playstation"))
        {
            p1Sprite.sprite = sony;
        }else if (name.Contains("switch") || name.Contains("pro"))
        {
            p1Sprite.sprite = nintendo;
        }
        else
        {
            p1Sprite.sprite = keyboard;
        }
    }

    public void UpdateP2Sprite()
    {
        InputDevice controller = GlobalData.Instance.Player2.Controller;
        if(controller == null)
        {
            p2Sprite.sprite = keyboard;
            return;
        }
        string name = controller.description.product.ToLower();
        if (name.Contains("xbox"))
        {
            p2Sprite.sprite = xbox;
        }
        else if (name.Contains("dualsense") || name.Contains("dualshock") || name.Contains("playstation"))
        {
            p2Sprite.sprite = sony;
        }
        else if (name.Contains("switch") || name.Contains("pro"))
        {
            p2Sprite.sprite = nintendo;
        }
        else
        {
            p2Sprite.sprite = keyboard;
        }
    }
}
