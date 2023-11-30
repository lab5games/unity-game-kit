using UnityEngine;

namespace Lab5Games.GameEvents
{
    [CreateAssetMenu(
        fileName ="New String Event",
        menuName =Constants.SCRIPT_EVENT_MENU_PATH + "String Event")]
    public class StringEvent : BaseScriptableEvent<string> { }
}