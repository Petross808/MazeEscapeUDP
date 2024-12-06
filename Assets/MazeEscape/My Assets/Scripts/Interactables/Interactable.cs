using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Interactable : EventListenerBase
{
    [SerializeField] private float _cooldown;
    private float _countdown;

    [EventSignature(typeof (GameObject), typeof(Item))]
    public override void OnEventRaised(GameEvent.CallbackContext context)
    {
        if (context.Get<GameObject>() == this.gameObject && _countdown <= 0)
        {
            Interact(context.Get<Item>());
            _countdown = _cooldown;
        }
    }

    public abstract void Interact(Item heldItem);

    private void Update()
    {
        if (_countdown > 0)
        {
            _countdown -= Time.deltaTime;
        }
    }

}
