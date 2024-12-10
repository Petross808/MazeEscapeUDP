using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    [SerializeField, EventSignature] private GameEvent _raiseOnEquip;


    public override void Interact(Item heldItem)
    {
        _raiseOnEquip.Raise(this);
    }
}
