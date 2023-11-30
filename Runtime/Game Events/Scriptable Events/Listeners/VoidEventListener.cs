using UnityEngine;
using UnityEngine.Events;

namespace Lab5Games.GameEvents
{
    [System.Serializable]
    public class VoidUnityEvent : UnityEvent<Void> { }

    [AddComponentMenu(Constants.SCRIPT_EVENT_MENU_PATH + "Void Event Listener")]
    public class VoidEventListener : BaseScriptableEventListener<Void, VoidEvent, VoidUnityEvent> { }
}