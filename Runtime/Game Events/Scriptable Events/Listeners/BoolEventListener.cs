using UnityEngine;
using UnityEngine.Events;

namespace Lab5Games.GameEvents
{
    [System.Serializable]
    public class BoolUnityEvent : UnityEvent<bool> { }

    [AddComponentMenu(Constants.SCRIPT_EVENT_MENU_PATH + "Boolean Event Listener")]
    public class BoolEventListener : BaseScriptableEventListener<bool, BoolEvent, BoolUnityEvent> { }
}