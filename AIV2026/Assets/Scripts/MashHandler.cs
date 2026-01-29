using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MashHandler : MonoBehaviour
{
    private Jammer player1;
    private Jammer player2;
    public delegate void OnP1Mash();
    public delegate void OnP2Mash();
    public float mashStrength;
    private float points = 0;
    public GameObject targetLeft;
    public GameObject targetRight;
    public GameObject divider;
    [Range(0f, 0.1f)]
    public float modifierRange;
    [Tooltip("Chance will be 1 in x")]
    public int randomAdvantage_chance;
    public bool randomizeAdvantage = true;
    public float advantageStrength;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        points = 0;
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        player1.Input.SwitchCurrentActionMap("Mash");
        player2.Input.SwitchCurrentActionMap("Mash");
    }

    // Update is called once per frame
    void Update()
    {
        divider.transform.position = new Vector3(points,divider.transform.position.y,divider.transform.position.z);
        Debug.Log("Kiss amount: " + points);
        if (divider.transform.position.x <= targetLeft.transform.position.x)
        {
            Debug.Log("player 2 wins");
        }
        if (divider.transform.position.x >= targetRight.transform.position.x)
        {
            Debug.Log("player 2 wins");
        }
    }

    private void FixedUpdate()
    {
        if (randomizeAdvantage && Random.Range(0, randomAdvantage_chance) == 0) //p1 gets a help!
        {
            points -= advantageStrength;
        }
        if (randomizeAdvantage && Random.Range(0, randomAdvantage_chance) == 0) //p2 gets a help!
        {
            points += advantageStrength;
        }


    }

    public void Onp1Mash()
    {
        points -= mashStrength + Random.Range(0, modifierRange);
    }

    public void Onp2Mash()
    {
        points += mashStrength + Random.Range(0, modifierRange);
    }
}
