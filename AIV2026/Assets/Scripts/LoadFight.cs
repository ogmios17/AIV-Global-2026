using System.Collections.Generic;
using UnityEngine;

public class LoadFight : MonoBehaviour
{
    [SerializeField]
    private Transform p1Position;
    [SerializeField]
    private Transform p2Position;

    private Jammer player1;
    private Jammer player2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        Instantiate(player1.CharacterPrefab, p1Position);
        Instantiate(player2.CharacterPrefab, p2Position).transform.rotation = new Quaternion(0,0,0,1); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
