using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField, EventSignature] GameEvent _onNewGameButtonClickedEvent;
    [SerializeField, EventSignature] GameEvent _onContinueButtonClickedEvent;
    [SerializeField, EventSignature] GameEvent _onSettingsButtonClickedEvent;
    [SerializeField, EventSignature] GameEvent _onButtonClickEvent;

    private UIDocument _document;
    private Button _newGameButton;
    private Button _continueButton;
    private Button _settingsButton;
    private Button _quitButton;

    private List<Button> _menuButtons = new();

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _newGameButton = _document.rootVisualElement.Q<Button>("PlayButton");
        _newGameButton.RegisterCallback<ClickEvent>(PlayNewGame);

        _continueButton = _document.rootVisualElement.Q<Button>("ContinueButton");
        _continueButton.RegisterCallback<ClickEvent>(ContinueGame);

        _settingsButton = _document.rootVisualElement.Q<Button>("SettingsButton");
        _settingsButton.RegisterCallback<ClickEvent>(OpenSettings);

        _quitButton = _document.rootVisualElement.Q<Button>("QuitButton");
        _quitButton.RegisterCallback<ClickEvent>(QuitGame);

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for(int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void PlayNewGame(ClickEvent evt)
    {
        _onNewGameButtonClickedEvent.Raise(this);
        _onContinueButtonClickedEvent.Raise(this);
    }

    private void ContinueGame(ClickEvent evt)
    {
        _onContinueButtonClickedEvent.Raise(this);
    }

    private void OpenSettings(ClickEvent evt)
    {
        _onSettingsButtonClickedEvent.Raise(this);
    }
    private void QuitGame(ClickEvent evt)
    {
        Application.Quit();
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        _onButtonClickEvent.Raise(this);
    }

    private void OnDestroy()
    {
        _newGameButton.UnregisterCallback<ClickEvent>(PlayNewGame);
        _continueButton.UnregisterCallback<ClickEvent>(ContinueGame);
        _settingsButton.UnregisterCallback<ClickEvent>(OpenSettings);

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }
}
