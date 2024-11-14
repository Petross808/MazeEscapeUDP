using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvent", order = 1)]
public class GameEvent : ScriptableGameObject
{
    private List<EventListener> _listeners = new();

    public void Raise(CallbackContext context)
    {
        for(int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(context);
        }
    }

    public void RegisterListener(EventListener listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void UnregisterListener(EventListener listener)
    {
        _listeners.Remove(listener);
    }


    public class CallbackContext
    {
        private readonly object _sender; 
        private readonly (string name, object value)[] _values;

        public object Sender => _sender;

        public CallbackContext(object sender, params (string name, object value)[] values)
        {
            _sender = sender; 
            _values = values;
        }

        /// <summary>Finds the first value that matches the type T.</summary>
        /// <returns>True, if value found, otherwise false.</returns>
        public bool ReadValue<T>(out T value)
        {
            for(int i = 0; i < _values.Length; i++)
            {
                if(_values[i].value is T)
                {
                    value = (T)(_values[i].value);
                    return true;
                }
            }

            value = default;
            return false;
        }

        /// <summary>Finds the first named value that matches the type T.</summary>
        /// <returns>True, if value found, otherwise false.</returns>
        public bool ReadValue<T>(string name, out T value)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                if (_values[i].name == name && _values[i].value is T)
                {
                    value = (T)(_values[i].value);
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}
