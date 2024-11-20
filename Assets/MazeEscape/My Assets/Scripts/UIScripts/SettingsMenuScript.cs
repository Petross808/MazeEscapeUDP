using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuScript : MonoBehaviour
{
    [SerializeField] GameEvent _onSettingsToggleEvent;

    private UIDocument _document;
    private Button _quitButton;
    //private AudioSource _audioSource;
    private Slider _masterSlider;
    private Slider _musicSlider;
    private Slider _sfxSlider;
    void Awake()
    {
        //_audioSource = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();
        _quitButton = _document.rootVisualElement.Q<Button>("Quit");
        _quitButton.RegisterCallback<ClickEvent>(CloseSettings);

        _masterSlider = _document.rootVisualElement.Q<Slider>("MasterSlider");
        _masterSlider.RegisterValueChangedCallback(evt => OnMasterVolumeChange(evt.newValue));

        _musicSlider = _document.rootVisualElement.Q<Slider>("MusicSlider");
        _musicSlider.RegisterValueChangedCallback(evt => OnMusicVolumeChange(evt.newValue));

        _sfxSlider = _document.rootVisualElement.Q<Slider>("SFXSlider");
        _sfxSlider.RegisterValueChangedCallback(evt => OnSFXVolumeChange(evt.newValue));
    }

    private void CloseSettings(ClickEvent evt)
    {
        //_audioSource.Play();
        _onSettingsToggleEvent.Raise(this, false);
    }

    private void OnDestroy()
    {
        _quitButton.UnregisterCallback<ClickEvent>(CloseSettings);
        _masterSlider.UnregisterValueChangedCallback(evt => OnMasterVolumeChange(evt.newValue));
        _musicSlider.UnregisterValueChangedCallback(evt => OnMusicVolumeChange(evt.newValue));
        _sfxSlider.UnregisterValueChangedCallback(evt => OnSFXVolumeChange(evt.newValue));
    }

    private void OnMasterVolumeChange(float value)
    {
        Debug.Log("Master Volume: " + value);
    }

    private void OnMusicVolumeChange(float value)
    {
        Debug.Log("Music Volume: " + value);
    }

    private void OnSFXVolumeChange(float value)
    {
        Debug.Log("SFX Volume: " + value);
    }

}
