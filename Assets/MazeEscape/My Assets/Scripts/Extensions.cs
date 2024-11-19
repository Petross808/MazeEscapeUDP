using UnityEngine;

public static class Extensions
{
    public static void EnsureSingleInstance(this MonoBehaviour behaviour)
    {
        System.Type type = behaviour.GetType();
        UnityEngine.Object[] objects = GameObject.FindObjectsByType(type, FindObjectsSortMode.None);
        if (objects.Length > 1)
        {
            throw new System.Exception($"EnsureSingleInstance on {type}: {objects.Length} instances found.");
        }
    }
}