using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEngine;

public class OnHitIFrames : EventListenerBase
{
    [SerializeField] float _immunityTime;
    [SerializeField] Collider _hurtboxCollider;

    public override void OnEventRaised(GameEvent.CallbackContext context)
    {
        StartIFrames(context);
    }

    public void StartIFrames(GameEvent.CallbackContext context)
    {
        if(context.Sender is MonoBehaviour script && script.gameObject == this.gameObject)
        {
            StartCoroutine(IFramesCoroutine());
        }
    }

    private IEnumerator IFramesCoroutine()
    {
        _hurtboxCollider.enabled = false;
        yield return new WaitForSeconds(_immunityTime);
        _hurtboxCollider.enabled = true;
    }
}
