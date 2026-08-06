using System.Collections;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private float timerBeforeHide;
    [SerializeField] private float targetPos;
    [SerializeField] private float startPos;
    [SerializeField] private float animSpeed;
    [SerializeField] private float acceleration;
    private float actualSpeed;
    private RectTransform rect;

    public void Awake()
    {
        rect = GetComponent<RectTransform>();
        actualSpeed = animSpeed;
    }
    public void OnEnable()
    {
        StopAllCoroutines();
        rect.anchoredPosition = new Vector2(startPos, rect.anchoredPosition.y);
        StartCoroutine(PopIn());
    }

    public IEnumerator HideAfterTimer()
    {
        yield return new WaitForSecondsRealtime(timerBeforeHide);
        StartCoroutine(PopOut());
        
    }

    public void HideGameObject()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator PopIn()
    {
        while(Mathf.Abs(rect.anchoredPosition.x - targetPos) > 0.1f)
        {
            rect.anchoredPosition = new Vector2(Mathf.MoveTowards(rect.anchoredPosition.x, targetPos, actualSpeed* Time.deltaTime), rect.anchoredPosition.y);
            actualSpeed += acceleration * Time.deltaTime;
            yield return null;
        }
        actualSpeed = animSpeed;
        rect.anchoredPosition = new Vector2(targetPos, rect.anchoredPosition.y);
        StartCoroutine(HideAfterTimer());
    }

    public IEnumerator PopOut()
    {
        while (Mathf.Abs(gameObject.transform.localPosition.x - startPos) > 0.1f)
        {
            rect.anchoredPosition = new Vector2(Mathf.MoveTowards(rect.anchoredPosition.x, startPos, actualSpeed * Time.deltaTime), rect.anchoredPosition.y);
            actualSpeed += acceleration * Time.deltaTime;
            yield return null;
        }
        actualSpeed = animSpeed;
        rect.anchoredPosition = new Vector2(startPos, rect.anchoredPosition.y);
        HideGameObject();
    }
}
