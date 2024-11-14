using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField] private GameEvent _listeningTo;
    [SerializeField] private UnityEvent<GameEvent.CallbackContext> _response;

    private void OnEnable() => _listeningTo?.RegisterListener(this);
    private void OnDisable() => _listeningTo.UnregisterListener(this);
    public void OnEventRaised(GameEvent.CallbackContext context) => _response?.Invoke(context);

}
