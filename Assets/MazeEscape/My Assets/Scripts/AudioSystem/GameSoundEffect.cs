using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSoundEffect", menuName = "GameSoundEffect", order = 1)]
public class GameSoundEffect : ScriptableGameObject
{
    [Space(10), Header("Sound Effect Data")]
    [SerializeField] bool _pickAtRandom = true;
    [SerializeField] bool _diegetic = true;
    [SerializeField] bool _isMusicTrack = false;
    [SerializeField] bool _isLooping = false;
    [SerializeField] List<AudioClip> _soundEffectVariants;

    public bool Diegetic => _diegetic;
    public bool IsMusicTrack => _isMusicTrack;
    public bool IsLooping => _isLooping;

    private int _currentIndex = -1;

    public AudioClip GetClip()
    {
        if (_soundEffectVariants.Count == 0)
            return null;

        if(_soundEffectVariants.Count == 1)
            return _soundEffectVariants[0];

        if (_pickAtRandom)
            return GetRandomVariant();

        return GetNextVariant();
    }

    private AudioClip GetNextVariant()
    {
        _currentIndex = (_currentIndex + 1) % _soundEffectVariants.Count;

        return _soundEffectVariants[_currentIndex];
    }
    private AudioClip GetRandomVariant()
    {
        int index = UnityEngine.Random.Range(0, _soundEffectVariants.Count);
        return _soundEffectVariants[index];
    }
}
