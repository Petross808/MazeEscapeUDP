using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    [SerializeField, EventSignature(typeof (GameObject), typeof (Item))] private GameEvent _interactEvent;
    [SerializeField] private float _interactReach;

    Item _heldItem;

    [EventSignature(typeof(Item))]
    public void OnItemEquip (GameEvent.CallbackContext context)
    {
        _heldItem = context.Get<Item>();
    }

    public void OnInteract()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactReach))
            _interactEvent.Raise(this, hit.collider.gameObject, _heldItem);
    }
}
