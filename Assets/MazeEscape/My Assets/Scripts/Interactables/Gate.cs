using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gate : Interactable
{
    [SerializeField] private bool _isOpen;
    [SerializeField] private bool _isLocked;
    [SerializeField] private Item _key;
    [SerializeField] private float _openingRate;
    [SerializeField] private float _openingAngle;
    [SerializeField] private GameObject _hinge;
    [SerializeField] private NavMeshObstacle _navMeshObstacle;

    [SerializeField, EventSignature] private GameEvent _gateUnlock;
    [SerializeField, EventSignature] private GameEvent _lockedGateInteract;
    [SerializeField, EventSignature] private GameEvent _unlockedGateInteract;
    [SerializeField, EventSignature(typeof (Item))] private GameEvent _itemConsume;

    private float _turnTarget = 0;
    private bool _isTurning = false;

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

        if (_isOpen)
            _turnTarget = _openingAngle;
        else
            _turnTarget = 0;

        _isTurning = true;
        StartCoroutine(TurnGate());
    }

    IEnumerator TurnGate()
    {

        while (_isTurning)
        {
            Debug.Log(_hinge.transform.rotation.eulerAngles.y);
            float turn = _openingRate * Time.deltaTime;
            if (Mathf.Abs(_hinge.transform.rotation.eulerAngles.y - _turnTarget) <= turn)
            {
                turn = Mathf.Abs(_hinge.transform.rotation.eulerAngles.y - _turnTarget); 
                _isTurning = false;
            }

            if (_hinge.transform.rotation.eulerAngles.y> _turnTarget)
                turn = -turn;

            _hinge.transform.Rotate(0, turn, 0);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}