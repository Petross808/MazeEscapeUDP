using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class TickSystem : MonoBehaviour
{
    [SerializeField] private float _tickTime;
    [SerializeField] private List<TickEvent> _tickEvents;

    private float _timer;
    private int _ticks;
    private int _maxTicks;

    void Awake()
    {
        _timer = 0;
        _maxTicks = 1;

        foreach (TickEvent e in _tickEvents)
        {
            if(_maxTicks % e.tickAmount != 0)
                _maxTicks *= e.tickAmount;
        }
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > _tickTime)
        {
            Tick();
            _timer -= _tickTime;
        }
    }

    private void Tick()
    {
        _ticks++;

        foreach (TickEvent e in _tickEvents)
        {
            if(_ticks % e.tickAmount == 0)
            {
                e.Raise(this);
            }
        }

        if (_ticks >= _maxTicks)
        {
            _ticks = 0;
        }
    }

    [Serializable]
    struct TickEvent
    {
        [Min(1)] public int tickAmount;
        [EventSignature] public GameEvent gameEvent;

        public void Raise(object sender)
        {
            gameEvent.Raise(sender);
        }
    }
}
