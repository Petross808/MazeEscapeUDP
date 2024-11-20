using UnityEngine;

public abstract class EventListenerBase : MonoBehaviour
{
    [SerializeField] protected GameEvent _listeningTo;

    private void OnEnable() => _listeningTo?.RegisterListener(this);
    private void OnDisable() => _listeningTo.UnregisterListener(this);
    public abstract void OnEventRaised(GameEvent.CallbackContext context);
}
