using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument)), RequireComponent(typeof(UiPanelScript))]
public class InGameUIScript : MonoBehaviour
{
    [SerializeField] private float _fadeTime;

    private UIDocument _document;
    private VisualElement _screenOverlay;

    void Awake()
    {
        _document = GetComponent<UIDocument>();
        _screenOverlay = _document.rootVisualElement.Q<VisualElement>("ScreenOverlay");
    }

    [EventSignature]
    public void FadeIn(GameEvent.CallbackContext _)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(Color.clear));
    }

    [EventSignature]
    public void FadeOut(GameEvent.CallbackContext _)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(Color.black));
    }

    IEnumerator FadeTo(Color target)
    {
        Color current = _screenOverlay.style.backgroundColor.value;
        float elapsed = 0;
        while(elapsed < _fadeTime)
        {
            elapsed += Time.deltaTime;
            _screenOverlay.style.backgroundColor = Color.Lerp(current, target, elapsed/_fadeTime);
            yield return null;
        }
    }
}
