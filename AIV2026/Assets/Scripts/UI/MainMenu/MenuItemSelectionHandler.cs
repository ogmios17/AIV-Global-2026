using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuItemSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float _verticalMoveAmount = 30f;
    [SerializeField] private float _moveDuration = 0.1f;
    [SerializeField, Range(0.0f, 2.0f)] private float _scaleAmount = 1.1f;

    private MenuItemSelectionManager _menuItemSelectionManager; // Usato per aggiornare nel manager l'ulitimo elemento selezionato
    private Vector3 _startPosition;
    private Vector3 _startScale;

    void Start()
    {
        // _startPosition = transform.position;
        // _startScale = transform.localScale;
        _menuItemSelectionManager = transform.parent.GetComponent<MenuItemSelectionManager>();
    }

    private IEnumerator MoveCard(bool startingAnimation)
    {
        Vector3 endPosition;
        Vector3 endScale;
        float elapsedTime = 0f;

        _startPosition = transform.position;
        _startScale = transform.localScale;

        while(elapsedTime < _moveDuration)
        {
            elapsedTime += Time.deltaTime;

            if(startingAnimation)
            {
                endPosition = _startPosition + new Vector3(0.0f, _verticalMoveAmount, 0.0f);
                endScale = _startScale * _scaleAmount;
            }

            else {
                endPosition = _startPosition - new Vector3(0.0f, _verticalMoveAmount, 0.0f);
                endScale = _startScale / _scaleAmount;
            }

            // Calculate the lerped amounts
            Vector3 lerpedPosition = Vector3.Lerp(transform.position, endPosition, (elapsedTime / _moveDuration));
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / _moveDuration));

            // Actually apply the changes to the position and scale
            transform.position = lerpedPosition;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCard(true));

        // Update the last selected item in the manager
        _menuItemSelectionManager.LastSelectedItem = gameObject;
        // Find the index
        for (int i = 0; i < _menuItemSelectionManager.MenuItems.Length; i++)
        {
            if (_menuItemSelectionManager.MenuItems[i] == gameObject)
            {
                _menuItemSelectionManager.LastSelectedIndex = i;
                break;
            }
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCard(false));
    }
}