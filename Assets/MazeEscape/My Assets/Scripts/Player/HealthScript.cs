using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private GameEvent _onDamageEvent;
    [SerializeField] private GameEvent _onDeathEvent;
    [SerializeField] private GameEvent _onHealEvent;

    private int _currentHealth;
    bool _alive;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public bool IsAlive => _alive;

    public void Start()
    {
        _currentHealth = _maxHealth;
        if (_currentHealth > 0)
            _alive = true;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
            return;

        int delta = _currentHealth;
        _currentHealth -= amount;
        _currentHealth = Mathf.Max(0, _currentHealth);
        delta -= _currentHealth;

        _onDamageEvent?.Raise(new(this, ("amount", delta)));

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        _alive = false;
        _onDeathEvent?.Raise(new(this));
    }

    public void HealDamage(int amount)
    {
        if (amount <= 0)
            return;

        int delta = _currentHealth;
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth);
        delta = _currentHealth - delta;

        _onHealEvent?.Raise(new(this, ("amount", delta)));
    }
}