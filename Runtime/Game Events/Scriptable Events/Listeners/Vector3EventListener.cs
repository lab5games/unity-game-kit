using UnityEngine;
using UnityEngine.Events;

namespace Lab5Games.GameEvents
{
    [System.Serializable]
    public class Vector3UnityEvent : UnityEvent<Vector3> { }

    [AddComponentMenu(Constants.SCRIPT_EVENT_MENU_PATH + "Vector3 Event Listener")]
    public class Vector3EventListener : BaseScriptableEventListener<Vector3, Vector3Event, Vector3UnityEvent> { }
}