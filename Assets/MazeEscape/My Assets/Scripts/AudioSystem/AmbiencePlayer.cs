using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AmbiencePlayer : MonoBehaviour
{
    [SerializeField, EventSignature] private GameEvent _ambienceEvent;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _distance;
    [SerializeField] private float _ambienceTimer;

    private float _angle;
    private Vector3 _position = new Vector3(0, 2, 0);
    private float _cooldown = 2;

    private void Start()
    {
        transform.parent = _player.transform;
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;

        if (_cooldown <= 0 )
        {
            _cooldown = Random.Range(_ambienceTimer / 2, _ambienceTimer);
            _angle = Random.Range(0, 360);
            _position.x = 0;
            _position.z = 0;
            _position.x = Mathf.Sin(_angle) * _distance;
            _position.z = Mathf.Cos(_angle) * _distance;
            transform.position = _position;
            _ambienceEvent.Raise(this);
        }
    }
}
