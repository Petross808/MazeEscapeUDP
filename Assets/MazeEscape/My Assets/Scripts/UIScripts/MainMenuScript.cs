using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    private UIDocument _document;
    private Button _playButton;
    private Button _settingsButton;
    private Button _quitButton;

    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();
        _playButton = _document.rootVisualElement.Q<Button>("PlayButton");
        _playButton.RegisterCallback<ClickEvent>(PlayGame);

        _settingsButton = _document.rootVisualElement.Q<Button>("SettingsButton");
        _settingsButton.RegisterCallback<ClickEvent>(OpenSettings);

        _quitButton = _document.rootVisualElement.Q<Button>("QuitButton");
        _quitButton.RegisterCallback<ClickEvent>(evt => Application.Quit());

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for(int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OpenSettings(ClickEvent evt)
    {
        Debug.Log("Open Settings");
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
       _audioSource.Play();
    }

    private void OnDestroy()
    {
        _playButton.UnregisterCallback<ClickEvent>(PlayGame);
        _settingsButton.UnregisterCallback<ClickEvent>(OpenSettings);

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void PlayGame(ClickEvent evt)
    {
        Debug.Log("Play Game");
    }
}
