using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SequenceHandler : MonoBehaviour
{
    private Jammer player1;
    private Jammer player2;
    [SerializeField]
    private int numberOfButtons;
    [SerializeField]
    [Tooltip("Final generated will be number +- modifier at random")]
    private int modifier;
    private Queue<string> sequence1 = new Queue<string>();
    private Queue<string> sequence2 = new Queue<string>();
    private string[] controllerInputs = {"/dpad/up",
                    "/dpad/down",
                    "/dpad/left",
                    "/dpad/right"
    };
    private string[] keyboardInputs = {"/up", "/down", "/left", "/right"
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        player1.Input.SwitchCurrentActionMap("Sequence");
        player2.Input.SwitchCurrentActionMap("Sequence");

        if (player1.Controller.Contains("Keyboard"))
            InitSetup(keyboardInputs, sequence1);
        else InitSetup(controllerInputs, sequence1);
        //player1.Input.actions.FindActionMap("Sequence").FindAction("Press").ApplyBindingOverride(sequence1.Dequeue());

        if (player2.Controller.Contains("Keyboard"))
            InitSetup(keyboardInputs, sequence2);
        else InitSetup(controllerInputs, sequence2);
        //player2.Input.actions.FindActionMap("Sequence").FindAction("Press").ApplyBindingOverride(sequence1.Dequeue());

    }

    private void InitSetup(string[] pool, Queue<string> sequence)
    {
        for (int i = 0; i < numberOfButtons; i++)
        {
            var index = Random.Range(0, pool.Length);
            sequence.Enqueue(pool[index]);
            Debug.Log("Tasto: " + pool[index]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onp1Press(string pressed)
    {
        // se pressed = dequeue allora tutto apposto senò muori
        if (pressed.ToLower().Contains(sequence1.Dequeue().ToLower()))
        {
            if (sequence1.Count <= 0)
            {
                Debug.Log("p1 wins!");
            }
        }
        else
        {
            Debug.Log("p1 loses...");
        }
    }

    public void Onp2Press(string pressed)
    {
        if (pressed.Contains(sequence2.Dequeue().ToLower()))
        {
            if (sequence2.Count <= 0)
            {
                Debug.Log("p2 wins!");
            }
        }
        else
        {
            Debug.Log("p2 loses...");
        }
    }
}
