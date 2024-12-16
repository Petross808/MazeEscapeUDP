using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    [SerializeField, EventSignature(typeof (GameObject), typeof (Item))] private GameEvent _interactEvent;
    [SerializeField] private GameObject _playerHead;
    [SerializeField] private float _interactReach;
    [SerializeField] private PlayerInventory _playerInventory;

    public void OnInteract()
    {
        if (Physics.Raycast(_playerHead.transform.position, _playerHead.transform.forward, out RaycastHit hit, _interactReach))
        { 
            _interactEvent.Raise(this, hit.collider.gameObject, _playerInventory.Hand);
        }
    }

    public void OnInspect()
    {
        _playerInventory.StartInspectItem();
    }

    public void OnInspectStop()
    {
        _playerInventory.StopInspectItem();
    }
}
