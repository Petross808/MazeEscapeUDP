using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Item _empty;

    private Item _hand;
    private List<Item> _journal = new List<Item> ();

    public Item Hand { get => _hand; set => _hand = value; }

    void Start()
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

}
