using UnityEngine;
using System;

namespace Lab5Games.GameEvents
{
    public abstract class BaseScriptableEvent<T> : ScriptableObject
    {
        event Action<T> _listeners;

        private void OnDisable()
        {
            _listeners = null;
        }

        public void Notify(T arg)
        {
            _listeners?.Invoke(arg);
        }

        public void AddListener(Action<T> action)
        {
            _listeners += action;
        }

        public void RemoveListener(Action<T> action)
        {
            _listeners -= action;
        }
    }
}
