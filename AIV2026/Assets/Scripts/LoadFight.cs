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
    void Start()
    {
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        player1.CharacterPrefab = Instantiate(GetPrefab(player1.CharacterType), p1Position);
        player2.CharacterPrefab = Instantiate(GetPrefab(player2.CharacterType), p2Position);
        player2.CharacterPrefab.transform.rotation = Quaternion.Euler(0, 180, 0);
        Animator[] animators = player1.CharacterPrefab.GetComponentsInChildren<Animator>();
        GlobalData.Instance.Player1.FighterAnim = animators[0];
        GlobalData.Instance.Player1.CardsAnim = animators[1];

        animators = player2.CharacterPrefab.GetComponentsInChildren<Animator>();
        GlobalData.Instance.Player2.FighterAnim = animators[0];
        GlobalData.Instance.Player2.CardsAnim = animators[1];

        // Aggancio la UI vita ai rispettivi Jammer (aggiornamento via evento).
        player1.CharacterPrefab.GetComponent<FightersDataBinder>().Bind(player1);
        player2.CharacterPrefab.GetComponent<FightersDataBinder>().Bind(player2);
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
