using System.Collections.Generic;
using UnityEngine;

public class LoadFight : MonoBehaviour
{
    [SerializeField]
    private Transform p1Position;
    [SerializeField]
    private Transform p2Position;
    [SerializeField]
    private Transform p1CardPosition;
    [SerializeField]
    private Transform p2CardPosition;
    [SerializeField]
    private GameObject notZilla;
    [SerializeField]
    private GameObject crackKen;

    private Jammer player1;
    private Jammer player2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        Debug.Log(player1.CharacterType + " "+ player2.CharacterType);
        player1.CharacterPrefab = Instantiate(GetPrefab(player1.CharacterType), p1Position);
        player2.CharacterPrefab = Instantiate(GetPrefab(player2.CharacterType), p2Position);
        player2.CharacterPrefab.transform.rotation = new Quaternion(0, 180, 0, 1);
        GlobalData.Instance.Player1.FighterAnim = player1.CharacterPrefab.GetComponentInChildren<Animator>();
        GlobalData.Instance.Player2.FighterAnim = player2.CharacterPrefab.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject GetPrefab(CharacterType type)
    {
        return type switch
        {
            CharacterType.NotZilla => notZilla,
            CharacterType.CrackKen => crackKen,
        };
    }
}
