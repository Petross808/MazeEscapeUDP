using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HazardScript : MonoBehaviour
{
    [SerializeField] int _hazardPower;
    [SerializeField] GameEvent _onHazardTriggerEvent;
    [SerializeField] LayerMask _targetLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (OnTargetLayer(other))
        {
            _onHazardTriggerEvent.Raise(this, other.gameObject, _hazardPower);
        }
    }

    private bool OnTargetLayer(Collider other) => (1 << other.gameObject.layer | _targetLayer) == _targetLayer;
}
