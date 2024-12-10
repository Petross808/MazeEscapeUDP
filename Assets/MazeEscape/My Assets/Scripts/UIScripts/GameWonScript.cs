using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument)), RequireComponent(typeof(UiPanelScript))]
public class GameWonScript : MonoBehaviour
{
    [SerializeField, EventSignature] GameEvent _onMenuButtonClickedEvent;
    [SerializeField, EventSignature] GameEvent _onButtonClickEvent;
    private UIDocument _document;
    private Button _homeButton;

    void Awake()
    {
        _document = GetComponent<UIDocument>();
        _homeButton = _document.rootVisualElement.Q<Button>("HomeButton");
        _homeButton.RegisterCallback<ClickEvent>(OpenMainMenu);
    }

    private void OpenMainMenu(ClickEvent evt)
    {
        _onMenuButtonClickedEvent.Raise(this);
        _onButtonClickEvent.Raise(this);
    }

    private void OnDestroy()
    {
        _homeButton.UnregisterCallback<ClickEvent>(OpenMainMenu);
    }
}
