using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuScript : MonoBehaviour
{
    private UIDocument _document;
    private Button _resumeButton;
    private Button _settingsButton;
    private Button _homeButton;

    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;

    void Awake()
    {
        _document = GetComponent<UIDocument>();
        _resumeButton = _document.rootVisualElement.Q<Button>("ResumeButton");
        _resumeButton.RegisterCallback<ClickEvent>(ResumeGame);
        
        _settingsButton = _document.rootVisualElement.Q<Button>("SettingsButton");
        _settingsButton.RegisterCallback<ClickEvent>(OpenSettings);
       
        _homeButton = _document.rootVisualElement.Q<Button>("HomeButton");
        _homeButton.RegisterCallback<ClickEvent>(OpenMainMenu);

        _audioSource = GetComponent<AudioSource>();

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OpenMainMenu(ClickEvent evt)
    {
        Debug.Log("Open Main Menu");
    }

    private void OpenSettings(ClickEvent evt)
    {
        Debug.Log("Open Settings");
    }

    private void ResumeGame(ClickEvent evt)
    {
        Debug.Log("Resume Game");
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
        _audioSource.Play();
    }
}
