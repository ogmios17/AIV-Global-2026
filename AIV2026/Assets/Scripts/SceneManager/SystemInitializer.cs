using UnityEngine;
using UnityEngine.AddressableAssets;

public static class SystemInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        GameObject gameObject = Addressables.InstantiateAsync("transition_system").WaitForCompletion();
        gameObject.name = "[SceneTransitionSystem]";
        Object.DontDestroyOnLoad(gameObject);
    }
}
