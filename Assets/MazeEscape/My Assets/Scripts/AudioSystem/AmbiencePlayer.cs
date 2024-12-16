using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    [SerializeField, EventSignature(typeof(GameSoundEffect), typeof(Vector3))] private GameEvent _soundEvent;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameSoundEffect _soundEffect;
    [SerializeField] private float _distance;
    [SerializeField] private int _ambienceTickTimer;

    private int _cooldown = 2;


    [EventSignature]
    public void Tick(GameEvent.CallbackContext _)
    {
        _cooldown--;

        if (_cooldown <= 0 )
        {
            _cooldown = Random.Range(_ambienceTickTimer / 2, _ambienceTickTimer);
            Vector3 pos = _player.transform.position + Random.onUnitSphere * _distance;
            _soundEvent.Raise(this, _soundEffect, pos);
        }
    }
}
