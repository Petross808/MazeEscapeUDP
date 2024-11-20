using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvent", order = 1)]
public class GameEvent : ScriptableGameObject
{
    private List<EventListenerBase> _listeners = new();

#if UNITY_EDITOR
    [SerializeField] private bool _debugLog;
#endif
    private void DebugLog(string message, params object[] values)
    {
    #if UNITY_EDITOR
        if(_debugLog)
            Debug.Log($"{this.name} {message}: '({string.Join(",", values)})'");
    #endif
    }

    [SerializeField] private List<ContextVariable> _contextVariables;

    private IReadOnlyList<(string Name, Type Type)> _context;

    public void Raise(object sender, params object[] parameters)
    {
        CallbackContext context = new CallbackContext(sender, this);
        context.SetParameters(parameters);
        for(int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(context);
        }
        DebugLog("Raise", parameters);
    }

    public void RegisterListener(EventListenerBase listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
        DebugLog("Register", listener);
    }

    public void UnregisterListener(EventListenerBase listener)
    {
        _listeners.Remove(listener);
        DebugLog("Unregister", listener);
    }

    public IReadOnlyList<(string Name, Type Type)> GetContextSignature()
    {
        if(_context == null )
        {
            List<(string Name, Type Type)> output = new();
            foreach (var contextVariable in _contextVariables)
            {
                Type type = System.Type.GetType(contextVariable.Type, false, true);
                if (type == null)
                    throw new System.Exception($"GameEvent '{this.name}': '{contextVariable.Type}' is not a valid type.");
                output.Add((contextVariable.Name, type));
            };
            _context = output;
        }
        return _context;
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        ClearConsole();
        _context = null;
        foreach (var contextVariable in _contextVariables)
        {
            Type type = System.Type.GetType(contextVariable.Type, false, true);
            if (type == null)
                throw new System.Exception($"GameEvent '{this.name}': '{contextVariable.Type}' is not a valid type.");
        }

        EventListener[] listeners = GameObject.FindObjectsByType<EventListener>(FindObjectsSortMode.None);
        foreach (var listener in listeners)
            listener.ValidateSignature();

        static void ClearConsole()
        {
            var assembly = Assembly.GetAssembly(typeof(SceneView));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);

        }
#endif
    }

    [Serializable] public struct ContextVariable
    {
        [Delayed] public string Name;
        [Delayed] public string Type;
    }

    public struct CallbackContext
    {
        private object _sender;
        private object[] _parameters;
        private GameEvent _gameEvent;

        public object Sender => _sender;

        public CallbackContext(object sender, GameEvent gameEvent)
        {
            _sender = sender;
            _gameEvent = gameEvent;
            _parameters = new object[gameEvent.GetContextSignature().Count];
        }

        public bool SetParameters(params object[] parameters)
        {
            Exception exception = new Exception(
                $"Cannot Raise GameEvent {_gameEvent.name}:" +
                $" Parameters '({string.Join(",", parameters)})' do not match the event signature '({string.Join(",", _gameEvent.GetContextSignature())})'.");
            if (parameters.Length != _parameters.Length)
                throw exception;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].GetType() == _gameEvent.GetContextSignature()[i].Type)
                {
                    _parameters[i] = parameters[i];
                }
                else
                {
                    throw exception;
                }
            }
            return true;
        }

        [MustMatchEventSignature]
        public T Get<T>(int index)
        {
            return (T) _parameters[index];
        }

        [MustMatchEventSignature]
        public T Get<T>()
        {
            foreach (var item in _parameters)
            {
                if (item is T)
                    return (T)item;
            }
            return default;
        }
    }
}
