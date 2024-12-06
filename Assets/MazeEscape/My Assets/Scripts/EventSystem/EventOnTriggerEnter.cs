using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EventOnTriggerEnter : MonoBehaviour
{
    [SerializeField, EventSignature(typeof(GameObject))] GameEvent _onTriggerEnterEvent;
    [SerializeField] LayerMask _targetLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (OnTargetLayer(other))
        {
            _onTriggerEnterEvent.Raise(this, other.gameObject);
        }
    }

    private bool OnTargetLayer(Collider other) => (1 << other.gameObject.layer | _targetLayer) == _targetLayer;
}
