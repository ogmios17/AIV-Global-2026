using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightersDataBinder : MonoBehaviour
{
    public List<GameObject> healthBars;
    public List<GameObject> cards;
    [SerializeField] private float healthLoseAnimationDuration;
    [SerializeField] private float shakeThreshold;
    [SerializeField] private float shakeSpeed;
    [SerializeField] private float colorChangeSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        cards[0].transform.parent.transform.rotation = new Quaternion(0, 0, 0, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHit(Jammer player)
    {
        var health = player.Health;

        if (health > 0)
        {
            Debug.Log("barra: ", healthBars[0]);
            Debug.Log("Taking a hit: how many bars left" + healthBars.Count + " index: " + (health - 1));
            StartCoroutine(ChangeHealthColorCO(healthBars[health - 1].GetComponent<SpriteRenderer>()));
            StartCoroutine(LoseHealthCO(healthBars[health - 1]));
            //healthBars[health - 1].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
            //healthBars.RemoveAt(health - 1);
            Debug.Log("new bars left: " + health);
        }

        player.TakeAHit();
    }

    public IEnumerator ChangeHealthColorCO(SpriteRenderer sprite)
    {
        while(sprite.color.r>0.01 || sprite.color.g >0.01 || sprite.color.b > 0.01)
        {
            Debug.Log("Changing color "+ sprite.color);
            sprite.color = new Color(Mathf.Lerp(sprite.color.r, 0, Time.deltaTime * colorChangeSpeed), Mathf.Lerp(sprite.color.g, 0, Time.deltaTime * colorChangeSpeed), Mathf.Lerp(sprite.color.b, 0, Time.deltaTime * colorChangeSpeed));
            yield return null;
        }
        sprite.color = new Color(0, 0, 0);
    }


    public IEnumerator LoseHealthCO(GameObject bar)
    {
        float timer = 0;
        Vector3 target, startPos;
        SpriteRenderer sprite = bar.GetComponent<SpriteRenderer>();
        startPos = bar.transform.localPosition;
        Debug.Log("Hello " + startPos);
        while(timer < healthLoseAnimationDuration)
        {
            //we randomly choose a target position inside given thresholds
            target = new Vector3(Random.Range(startPos.x - shakeThreshold, startPos.x + shakeThreshold), Random.Range(startPos.y - shakeThreshold, startPos.y + shakeThreshold), startPos.z);
            
            //we move to that position
            while(Mathf.Abs(bar.transform.localPosition.x- target.x)>0.1f || Mathf.Abs(bar.transform.localPosition.y - target.y) > 0.1f)
            {
                timer += Time.deltaTime;
                bar.transform.localPosition = new Vector3(Mathf.Lerp(bar.transform.localPosition.x, target.x, Time.deltaTime * shakeSpeed), Mathf.Lerp(bar.transform.localPosition.y, target.y, Time.deltaTime * shakeSpeed), startPos.z);
                yield return null;
            }

            //repeat after 1 frame
            yield return null;
        }

        //reset position
        bar.transform.localPosition = startPos;
        Debug.Log("Hello 2" + startPos + bar.transform.localPosition);
    }
}
