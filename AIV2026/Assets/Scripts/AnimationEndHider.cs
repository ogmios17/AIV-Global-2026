using System.Collections.Generic;
using UnityEngine;

public class AnimationEndHider : MonoBehaviour
{
    [SerializeField] private List<GameObject> hide;

    public void OnAnimationFinishHide()
    {
        for (int i = 0; i < hide.Count; i++)
        {
            hide[i].SetActive(false);
        }
    }

}
