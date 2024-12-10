using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UiPanelScript : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private int _priority;
    [SerializeField] private bool _closeableByInput;
    [SerializeField] private bool _pausesGame;
    [SerializeField] private bool _invertSetOpen;

    private bool _open;
    private UIDocument _document;

    public bool IsOpen => _open;
    public int Priority => _priority;
    public bool CloseableByInput => _closeableByInput;
    public bool PausesGame => _pausesGame;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _open = false;
        _document.rootVisualElement.visible = false;
        _uiManager.UiScripts.Add(this);
    }

    [EventSignature]
    public void Open(GameEvent.CallbackContext _) => Open();

    [EventSignature]
    public void Close(GameEvent.CallbackContext _) => Close();

    [EventSignature(typeof(bool))]
    public void SetOpen(GameEvent.CallbackContext context)
    {
        bool setState = context.Get<bool>();
        if(setState ^ _invertSetOpen)
        {
            Open(context);
        }
        else
        {
            Close(context);
        }
    }

    private void Open()
    {
        _document.sortingOrder = _priority;
        _open = true;
        _document.rootVisualElement.visible = true;
        _uiManager.OnUIChanged();
    }

    private void Close()
    {
        _open = false;
        _document.rootVisualElement.visible = false;
        _uiManager.OnUIChanged();
    }
}
