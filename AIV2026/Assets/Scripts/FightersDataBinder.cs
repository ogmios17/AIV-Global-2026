using System.Collections.Generic;
using UnityEngine;

public class FightersDataBinder : MonoBehaviour
{
    public List<GameObject> healthBars;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            healthBars[health - 1].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
            //healthBars.RemoveAt(health - 1);
            Debug.Log("new bars left: " + health);
        }

        player.TakeAHit();
    }
}
