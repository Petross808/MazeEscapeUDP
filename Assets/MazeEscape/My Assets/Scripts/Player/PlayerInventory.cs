using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField, EventSignature(typeof (Item))] private GameEvent _raiseOnEquip;
    [SerializeField] private Item _empty;

    private Item _hand;
    private List<Item> _journal = new List<Item> ();

    void Start()
    {
        _hand = _empty;
        _raiseOnEquip.Raise(this, _hand);
    }

    public void ConsumeItem()
    {
        _hand.gameObject.SetActive (false);
        _hand = _empty;
    }

}
