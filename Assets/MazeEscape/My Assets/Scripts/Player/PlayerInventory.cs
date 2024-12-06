using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField, EventSignature(typeof (Item))] private GameEvent _raiseOnEquip;
    [SerializeField] private Item _hand;
    List<Item> _journal = new List<Item> ();

    void Start()
    {
        _raiseOnEquip.Raise(this, _hand);
    }


}
