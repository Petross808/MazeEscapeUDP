using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument)), RequireComponent(typeof(UiPanelScript))]
public class InGameUIScript : MonoBehaviour
{
    [SerializeField] private float _fadeTime;

    private UIDocument _document;
    private VisualElement _screenOverlay;
    private VisualElement _crosshair;
    private VisualElement _interactCrosshair;
    private VisualElement _itemHints;

    void Awake()
    {
        _document = GetComponent<UIDocument>();
        _screenOverlay = _document.rootVisualElement.Q<VisualElement>("ScreenOverlay");
        _crosshair = _document.rootVisualElement.Q<VisualElement>("CrossHair");
        _interactCrosshair = _document.rootVisualElement.Q<VisualElement>("InteractCrosshair");
        _itemHints = _document.rootVisualElement.Q<VisualElement>("ItemHints");
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

    [EventSignature]
    public void ShowCrosshair(GameEvent.CallbackContext _)
    {
        _crosshair.visible = true;
        _interactCrosshair.visible = false;
    }

    [EventSignature]
    public void ShowInteractCrosshair(GameEvent.CallbackContext _)
    {
        _crosshair.visible = false;
        _interactCrosshair.visible = true;
    }

    [EventSignature]
    public void ShowItemHints(GameEvent.CallbackContext _)
    {
        _itemHints.visible = true;
    }

    [EventSignature(typeof(Item))]
    public void HideItemHints(GameEvent.CallbackContext _)
    {
        _itemHints.visible = false;
    }
}
