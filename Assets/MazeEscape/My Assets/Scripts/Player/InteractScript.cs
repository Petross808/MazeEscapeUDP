using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    [SerializeField, EventSignature(typeof (GameObject), typeof (Item))] private GameEvent _interactEvent;
    [SerializeField, EventSignature] private GameEvent _onAimAtInteractable;
    [SerializeField, EventSignature] private GameEvent _onAimAtNonInteractable;
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

    [EventSignature]
    public void CheckInteract(GameEvent.CallbackContext _)
    {
        if (Physics.Raycast(_playerHead.transform.position, _playerHead.transform.forward, out RaycastHit hit, _interactReach) &&
            hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable inter) &&
            inter != _playerInventory.Hand)
        {
            _onAimAtInteractable.Raise(this);
        }
        else
        {
            _onAimAtNonInteractable.Raise(this);
        }
    }
}
