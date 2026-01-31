using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class MenuItemSelectionManager : MonoBehaviour
{
    public GameObject[] MenuItems;
    
    public GameObject LastSelectedItem { get; set; }
    public int LastSelectedIndex { get; set; }

    void Awake()
    {
        if (MenuItems == null || MenuItems.Length == 0)
        {
            // Prende tutti i Button nei figli (anche nei figli dei figli)
            Button[] buttons = GetComponentsInChildren<Button>();

            MenuItems = new GameObject[buttons.Length];

            for (int i = 0; i < buttons.Length; i++)
                MenuItems[i] = buttons[i].gameObject;
        }

    }

    private void OnEnable()
    {
        StartCoroutine(SetSelectedAfterOneFrame(MenuItems[0]));
    }

    private void Update()
    {
        // If we move right
        //if (InputManager.Instance.NavigationInput.x > 0)
        //{
        //    // Select the next menu item
        //    HandleNextMenuItemSelection(1);
        //}
        //else if (InputManager.Instance.NavigationInput.x < 0)
        //{
        //    // Select the previous menu item
        //    HandleNextMenuItemSelection(-1);
        //}

        // If we move left
    }

    private IEnumerator SetSelectedAfterOneFrame(GameObject menuItem)
    {
        yield return null; // Wait for one frame
        yield return null; // Wait for one frame
        EventSystem.current.SetSelectedGameObject(menuItem);
    }

    private void HandleNextMenuItemSelection(int addition)
    {
        if (EventSystem.current.currentSelectedGameObject == null && LastSelectedItem != null)
        {
            int newIndex = LastSelectedIndex + addition;
            newIndex = Mathf.Clamp(newIndex, 0, MenuItems.Length - 1);
            EventSystem.current.SetSelectedGameObject(MenuItems[newIndex]);
        }
    }
}
