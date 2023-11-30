using UnityEngine;

namespace Lab5Games.GameEvents
{
    [CreateAssetMenu(
        fileName ="New Int Event",
        menuName =Constants.SCRIPT_EVENT_MENU_PATH + "Int Event")]
    public class IntEvent : BaseScriptableEvent<int> { }
}