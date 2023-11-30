using UnityEngine;
using UnityEngine.Events;

namespace Lab5Games.GameEvents
{
    [System.Serializable]
    public class StringUnityEvent : UnityEvent<string> { }

    [AddComponentMenu(Constants.SCRIPT_EVENT_MENU_PATH + "String Event Listener")]
    public class StringEventListener : BaseScriptableEventListener<string, StringEvent, StringUnityEvent> { }
}