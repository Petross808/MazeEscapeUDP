using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HealthScript : MonoBehaviour, ISaveData
{
    [SerializeField] private int _maxHealth;
    [SerializeField, EventSignature(typeof(int))] private GameEvent _onDamageEvent;
    [SerializeField, EventSignature(typeof(GameObject))] private GameEvent _onDeathEvent;
    [SerializeField, EventSignature(typeof(int))] private GameEvent _onHealEvent;

    private int _currentHealth;
    bool _alive;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public bool IsAlive => _alive;

    public void Awake()
    {
        _currentHealth = _maxHealth;
        if (_currentHealth > 0)
            _alive = true;
    }

    public void SetRawHealth(int value)
    {
        _currentHealth = value;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        if (_currentHealth > 0)
            _alive = true;
    }

    public void TakeDamage(int amount, GameObject source)
    {
        if (amount <= 0)
            return;

        int delta = _currentHealth;
        _currentHealth -= amount;
        _currentHealth = Mathf.Max(0, _currentHealth);
        delta -= _currentHealth;

        _onDamageEvent.Raise(this, delta);

        if (_currentHealth <= 0)
            Die(source);
    }

    private void Die(GameObject source)
    {
        _alive = false;
        _onDeathEvent.Raise(this, source);
    }

    public void HealDamage(int amount)
    {
        if (amount <= 0)
            return;

        int delta = _currentHealth;
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth);
        delta = _currentHealth - delta;

        _onHealEvent.Raise(this, delta);
    }

    [EventSignature(typeof(GameObject))]
    public void TryTakeDamage(GameEvent.CallbackContext context)
    {
        GameObject objectToTakeDamage = context.Get<GameObject>();
        GameObject source = (context.Sender as MonoBehaviour)?.gameObject;

        if (objectToTakeDamage == this.gameObject)
        {
            TakeDamage(1, source);
        }
    }

    public void LoadData(SaveData data)
    {
        this._currentHealth = data.playerCurrentHealth;
        this._alive = data.playerAlive;
    }

    public void SaveData(ref SaveData data)
    {
        data.playerCurrentHealth = this._currentHealth;
        data.playerAlive = this._alive;
    }
}