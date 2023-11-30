using UnityEngine;
using UnityEngine.Events;

namespace Lab5Games.GameEvents
{
    public abstract class BaseScriptableEventListener<T, E, U> : MonoBehaviour
        where E : BaseScriptableEvent<T>
        where U : UnityEvent<T>
    {
        public E Event;
        public U UnityEvent;

        protected virtual void OnEnable()
        {
            Event.AddListener(OnNotify);
        }

        protected virtual void OnDisable()
        {
            Event.RemoveListener(OnNotify);
        }

        public virtual void OnNotify(T arg)
        {
            UnityEvent.Invoke(arg);
        }
    }
}
