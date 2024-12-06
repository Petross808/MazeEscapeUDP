using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gate : Interactable
{
    [SerializeField] private bool _isLocked;
    [SerializeField] private bool _isOpen;
    [SerializeField] private Item _key;
    [SerializeField] private NavMeshObstacle _navMeshObstacle;

    [SerializeField, EventSignature] private GameEvent _gateUnlock;
    [SerializeField, EventSignature] private GameEvent _lockedGateInteract;
    [SerializeField, EventSignature] private GameEvent _unlockedGateInteract;
    [SerializeField, EventSignature(typeof (Item))] private GameEvent _itemConsume;

    public override void Interact(Item heldItem)
    {
        if (_isLocked)
        {
            if (heldItem == _key)
            {
                _isLocked = false;
                _gateUnlock.Raise(this);
                _itemConsume.Raise(this, _key);
            }
            else
            {
                _lockedGateInteract.Raise(this);
            }
        }
        else
        {
            Toggle();
            _unlockedGateInteract.Raise(this);
        }
    }

    public void Toggle()
    {
        _navMeshObstacle.enabled = _isOpen;
        _isOpen = !_isOpen;
    }
}