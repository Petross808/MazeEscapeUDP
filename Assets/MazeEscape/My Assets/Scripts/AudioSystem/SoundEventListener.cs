using UnityEngine;

public class SoundEventListener : EventListenerBase
{
    [SerializeField] GameSoundEffect _soundEffect;
    [SerializeField, EventSignature(typeof(GameSoundEffect), typeof(Vector3))] GameEvent _onSoundPlayRequestEvent;

    public override void OnEventRaised(GameEvent.CallbackContext context)
    {
        _onSoundPlayRequestEvent.Raise(this, _soundEffect, transform.position);
    }
}
