using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField, EventSignature(typeof(Item))] private GameEvent _dropEvent;
    [SerializeField] private Item _empty;
    [SerializeField] private GameObject _inspect;

    private Item _hand;
    private List<Item> _journal = new List<Item> ();

    public Item Hand { get => _hand; set => _hand = value; }

    void Awake()
    {
        _hand = _empty;
    }

    [EventSignature]
    public void PickUpItem(GameEvent.CallbackContext context)
    {
        if (_hand == _empty)
        {
            Item item = context.Sender as Item;
            _hand = item;
            item.gameObject.transform.position = _empty.transform.position;
            item.gameObject.transform.rotation = _empty.transform.rotation;
            item.gameObject.transform.parent = _empty.transform;
        }
    }

    [EventSignature(typeof(Item))]
    public void ConsumeItem(GameEvent.CallbackContext context)
    {
        context.Get<Item>().gameObject.SetActive(false);
        _hand = _empty;
    }

    public void DropItem()
    {
        if (_hand != _empty)
        {
            StopInspectItem();
            _hand.transform.position = new Vector3 (transform.position.x, -0.5f, transform.position.z);
            _hand.transform.rotation = transform.rotation;
            _hand.transform.Rotate(0, _empty.transform.rotation.eulerAngles.y, 0);
            _hand.transform.parent = null;
            _dropEvent.Raise(this, _hand);
            _hand = _empty;
        }
    }

    public void StartInspectItem()
    {
        if (_hand == _empty) return;
        _hand.transform.position = _inspect.transform.position;
        _hand.transform.rotation = _inspect.transform.rotation;
    }

    public void StopInspectItem()
    {
        if (_hand == null) return;
        if (_hand == _empty) return;
        _hand.transform.position = _empty.transform.position;
        _hand.transform.rotation = _empty.transform.rotation;
    }

}
