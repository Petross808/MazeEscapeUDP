using UnityEngine;

public class ScriptableGameObject : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField] private string _editorName;
    [SerializeField, TextArea(3,10)] private string _editorDescription;
#endif
}
