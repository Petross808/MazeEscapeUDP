using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioSystemScript : MonoBehaviour
{
    [SerializeField] int _initialPoolSize;

    [SerializeField] AudioSource _audioSourceTemplate;

    List<AudioSource> _audioSourcePool;

    private float _masterVolume = 0.5f;
    private float _sfxVolume = 0.5f;
    private float _musicVolume = 0.5f;

    void Awake()
    {
        this.EnsureSingleInstance();
        _audioSourcePool = new List<AudioSource>();
        for (int i = 0; i < _initialPoolSize; i++)
        {
            AddNewToPool();
        }
    }

    private AudioSource AddNewToPool()
    {
        AudioSource audioSource = GameObject.Instantiate<AudioSource>(_audioSourceTemplate, gameObject.transform);
        _audioSourcePool.Add(audioSource);
        return audioSource;
    }

    private AudioSource GetAvailable()
    {
        foreach(AudioSource audioSource in _audioSourcePool)
        {
            if(!audioSource.isPlaying)
                return audioSource;
        }

        return AddNewToPool();
    }

    [EventSignature]
    public void StopAllPlaying(GameEvent.CallbackContext context)
    {
        foreach(var audioSource in _audioSourcePool)
        {
            audioSource.Stop();
        }
    }

    [EventSignature]
    public void StopAllPlayingOnLoop(GameEvent.CallbackContext context)
    {
        foreach (var audioSource in _audioSourcePool)
        {
            if(audioSource.isPlaying && audioSource.loop)
            {
                audioSource.Stop();
            }
        }
    }

    [EventSignature(typeof(GameSoundEffect), typeof(Vector3))]
    public void PlaySoundEffect(GameEvent.CallbackContext context)
    {
        GameSoundEffect soundEffect = context.Get<GameSoundEffect>();
        Vector3 position = context.Get<Vector3>();
        AudioSource audioSource = GetAvailable();

        audioSource.transform.position = position;
        audioSource.clip = soundEffect.GetClip();
        audioSource.volume = _masterVolume * (soundEffect.IsMusicTrack ? _musicVolume : _sfxVolume);
        audioSource.spatialBlend = soundEffect.Diegetic ? 1 : 0;
        audioSource.loop = soundEffect.IsMusicTrack;
        audioSource.Play();
    }

    [EventSignature(typeof(float))]
    public void SetMasterVolume(GameEvent.CallbackContext context)
    {
        _masterVolume = context.Get<float>();
    }

    [EventSignature(typeof(float))]
    public void SetSFXVolume(GameEvent.CallbackContext context)
    {
        _sfxVolume = context.Get<float>();
    }

    [EventSignature(typeof(float))]
    public void SetMusicVolume(GameEvent.CallbackContext context)
    {
        _musicVolume = context.Get<float>();
    }
}
