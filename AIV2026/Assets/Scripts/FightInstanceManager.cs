using UnityEngine;

public class FightInstanceManager : MonoBehaviour
{
    public GameObject prefab;
    private GameObject prefabClone;

    void Start()
    {
        prefabClone = Instantiate(prefab, new Vector3(0, 0, 35), Quaternion.identity);
        prefabClone.name = "Fight";

        // Expose the fight root so minigame states can position themselves relative to it
        // without a fragile GameObject.Find("Fight") lookup.
        GlobalData.Instance.miniGameTransform = prefabClone.transform;
    }
}
