using UnityEngine;

namespace Lab5Games.GameEvents
{
    [CreateAssetMenu(
        fileName = "New Bool Event",
        menuName = Constants.SCRIPT_EVENT_MENU_PATH + "Bool Event")]
    public class BoolEvent : BaseScriptableEvent<bool> { }
}