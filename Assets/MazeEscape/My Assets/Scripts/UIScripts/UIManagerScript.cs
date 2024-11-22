using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameEvent _onUiChanged;

    private List<UiPanelScript> _uiScripts = new();
    public List<UiPanelScript> UiScripts => _uiScripts;

    private void Awake()
    {
        this.EnsureSingleInstance();
    }

    [EventSignature]
    public void CloseTopMostCloseable(GameEvent.CallbackContext context)
    {
        List<UiPanelScript> closeable = _uiScripts.FindAll(a => a.IsOpen && a.CloseableByInput);
        if (closeable.Count == 0)
            return;

        UiPanelScript topmost = closeable[0];
        foreach (UiPanelScript script in closeable)
        {
            if(script.Priority > topmost.Priority)
                topmost = script;
        }

        topmost.Close(context);
    }

    [EventSignature]
    public void ForceCloseAll(GameEvent.CallbackContext context)
    {
        foreach (UiPanelScript script in _uiScripts)
        {
            script.Close(context);
        }
    }

    public void OnUIChanged()
    {
        foreach(UiPanelScript script in _uiScripts)
        {
            if(script.IsOpen && script.PausesGame)
            {
                _onUiChanged.Raise(this, true);
                return;
            }
        }

        _onUiChanged.Raise(this, false);
    }

}
