using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument)), RequireComponent(typeof(UiPanelScript))]
public class PauseMenuScript : MonoBehaviour
{
    [SerializeField, EventSignature] GameEvent _onMenuButtonClickedEvent;
    [SerializeField, EventSignature] GameEvent _onSettingsButtonClickedEvent;
    [SerializeField, EventSignature] GameEvent _onButtonClickEvent;
    private UiPanelScript _uiPanelScript;

    private UIDocument _document;
    private Button _resumeButton;
    private Button _settingsButton;
    private Button _homeButton;

    private List<Button> _menuButtons = new();

    void Awake()
    {
        _uiPanelScript = GetComponent<UiPanelScript>();
        _document = GetComponent<UIDocument>();
        _resumeButton = _document.rootVisualElement.Q<Button>("ResumeButton");
        _resumeButton.RegisterCallback<ClickEvent>(ResumeGame);
        
        _settingsButton = _document.rootVisualElement.Q<Button>("SettingsButton");
        _settingsButton.RegisterCallback<ClickEvent>(OpenSettings);
       
        _homeButton = _document.rootVisualElement.Q<Button>("HomeButton");
        _homeButton.RegisterCallback<ClickEvent>(OpenMainMenu);

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OpenMainMenu(ClickEvent evt)
    {
        _onMenuButtonClickedEvent.Raise(this);
    }

    private void OpenSettings(ClickEvent evt)
    {
        _onSettingsButtonClickedEvent.Raise(this);
    }

    private void ResumeGame(ClickEvent evt)
    {
        _uiPanelScript.Close(new());
    }

    private void OnDestroy()
    {
        _resumeButton.UnregisterCallback<ClickEvent>(ResumeGame);
        _settingsButton.UnregisterCallback<ClickEvent>(OpenSettings);
        _homeButton.UnregisterCallback<ClickEvent>(OpenMainMenu);

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        _onButtonClickEvent.Raise(this);
    }
}
