using UnityEngine;
using UnityEngine.Events;

namespace Lab5Games.GameEvents
{
    [System.Serializable]
    public class IntUnityEvent : UnityEvent<int> { }

    [AddComponentMenu(Constants.SCRIPT_EVENT_MENU_PATH + "Int Event Listener")]
    public class IntEventListener : BaseScriptableEventListener<int, IntEvent, IntUnityEvent> { }
}