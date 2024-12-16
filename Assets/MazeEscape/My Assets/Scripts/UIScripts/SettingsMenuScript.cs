using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuScript : MonoBehaviour
{
    [SerializeField, EventSignature(typeof(float))] GameEvent _onMasterVolumeSliderChange;
    [SerializeField, EventSignature(typeof(float))] GameEvent _onMusicVolumeSliderChange;
    [SerializeField, EventSignature(typeof(float))] GameEvent _onSFXVolumeSliderChange;
    [SerializeField, EventSignature] GameEvent _onButtonClickEvent;

    private UiPanelScript _uiPanelScript;
    private UIDocument _document;
    private Button _quitButton;
    private Slider _masterSlider;
    private Slider _musicSlider;
    private Slider _sfxSlider;
    void Awake()
    {
        _uiPanelScript = GetComponent<UiPanelScript>();
        _document = GetComponent<UIDocument>();
        _quitButton = _document.rootVisualElement.Q<Button>("Quit");
        _quitButton.RegisterCallback<ClickEvent>(CloseSettings);

        _masterSlider = _document.rootVisualElement.Q<Slider>("MasterSlider");
        _masterSlider.RegisterValueChangedCallback(evt => OnMasterVolumeChange(evt.newValue));

        _musicSlider = _document.rootVisualElement.Q<Slider>("MusicSlider");
        _musicSlider.RegisterValueChangedCallback(evt => OnMusicVolumeChange(evt.newValue));

        _sfxSlider = _document.rootVisualElement.Q<Slider>("SFXSlider");
        _sfxSlider.RegisterValueChangedCallback(evt => OnSFXVolumeChange(evt.newValue));

        if (PlayerPrefs.HasKey("masterVolume"))
            _masterSlider.value = PlayerPrefs.GetFloat("masterVolume") * 10;
        if (PlayerPrefs.HasKey("musicVolume"))
            _musicSlider.value = PlayerPrefs.GetFloat("musicVolume") * 10;
        if (PlayerPrefs.HasKey("sfxVolume"))
            _sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume") * 10;
    }

    private void CloseSettings(ClickEvent evt)
    {
        _onButtonClickEvent.Raise(this);
        _uiPanelScript.Close(new());
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
        _onMasterVolumeSliderChange.Raise(this, value/10);
    }

    private void OnMusicVolumeChange(float value)
    {
        _onMusicVolumeSliderChange.Raise(this, value / 10);
    }

    private void OnSFXVolumeChange(float value)
    {
        _onSFXVolumeSliderChange.Raise(this, value / 10);
    }

}
