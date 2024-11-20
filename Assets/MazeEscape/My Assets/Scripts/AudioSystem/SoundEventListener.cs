using UnityEngine;

public class SoundEventListener : EventListenerBase
{
    [SerializeField] GameSoundEffect _soundEffect;
    [SerializeField] GameEvent _onSoundPlayRequestEvent;

    public override void OnEventRaised(GameEvent.CallbackContext context)
    {
        _onSoundPlayRequestEvent.Raise(this, _soundEffect, transform.position);
    }
}
