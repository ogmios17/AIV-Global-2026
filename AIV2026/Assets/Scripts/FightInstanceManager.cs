using UnityEngine;

public class FightInstanceManager : MonoBehaviour
{
    public GameObject prefab;
    private GameObject prefabClone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prefabClone = Instantiate(prefab, new Vector3(0, 0, 35), Quaternion.identity);
        prefabClone.name = "Fight";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
