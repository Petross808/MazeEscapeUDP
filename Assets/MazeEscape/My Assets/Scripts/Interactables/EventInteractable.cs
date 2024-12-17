using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInteractable : Interactable
{
    [SerializeField, EventSignature] private GameEvent _raiseOnInteract;
    [SerializeField] private int _uses;

    public override void Interact(Item heldItem)
    {
        if (_uses != 0)
        {
            _raiseOnInteract.Raise(this);
            _uses--;
        }
    }
}
