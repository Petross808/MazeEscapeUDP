using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField] private GameEvent _listeningTo;
    [SerializeField] private UnityEvent<GameEvent.CallbackContext> _response;

    private void OnEnable() => _listeningTo?.RegisterListener(this);
    private void OnDisable() => _listeningTo.UnregisterListener(this);
    public void OnEventRaised(GameEvent.CallbackContext context) => _response?.Invoke(context);

    public void OnValidate()
    {
        if (_listeningTo == null)
            throw new System.NullReferenceException($"No event set in EventListener '{this.name}'.");

        for (int i = 0; i < _response.GetPersistentEventCount(); i++)
        {
            string methodName = _response.GetPersistentMethodName(i);
            UnityEngine.Object target = _response.GetPersistentTarget(i);
            EventSignatureAttribute attribute = target?.GetType()?.GetMethod(methodName).GetCustomAttribute<EventSignatureAttribute>();
            if (attribute == null)
                throw new System.Exception(
                    $"Cannot register GameEvent to EventListener on GameObject '{this.name}'\n" +
                    $"EventSignature attribute not found on response at index {i} ('{target}.{methodName}').");

            System.Type[] responseSignature = attribute.Signature;
            List<System.Type> eventSignature = new();
            foreach (var e in _listeningTo.GetContextSignature())
                eventSignature.Add(e.Type);

            Exception exception = new(
                $"Cannot register GameEvent to EventListener on GameObject '{ this.name }'\n" +
                $"GameEvent does not match signature of response at index {i}\n" +
                $"{_listeningTo.name}: [{string.Join(",", eventSignature)}]\n" +
                $"{methodName}: [{string.Join<Type>(",", responseSignature)}]"
                );

            if (responseSignature.Length != eventSignature.Count)
                throw exception;

            for(int j = 0; j < responseSignature.Length; j++)
            {
                if (responseSignature[j] != eventSignature[j])
                    throw exception;
            }
        }
    }
}
