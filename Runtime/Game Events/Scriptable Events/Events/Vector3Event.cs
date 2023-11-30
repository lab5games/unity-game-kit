using UnityEngine;

namespace Lab5Games.GameEvents
{
    [CreateAssetMenu(
        fileName ="New Vector3 Event",
        menuName =Constants.SCRIPT_EVENT_MENU_PATH + "Vector3 Event")]
    public class Vector3Event : BaseScriptableEvent<Vector3> { }
}