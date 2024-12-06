using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Interactable : EventListenerBase
{
    [EventSignature(typeof (GameObject), typeof(Item))]
    public override void OnEventRaised(GameEvent.CallbackContext context)
    {
        if (context.Get<GameObject>() == this.gameObject)
        {
            Interact(context.Get<Item>());
        }
    }

    public abstract void Interact(Item heldItem);
}
