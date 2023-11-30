using UnityEngine;

namespace Lab5Games.GameEvents
{
    public struct Void
    {
        public readonly static Void data = new Void();
    }

    [CreateAssetMenu(
        fileName ="New Void Event",
        menuName =Constants.SCRIPT_EVENT_MENU_PATH + "Void Event")]
    public class VoidEvent : BaseScriptableEvent<Void>
    {
        public void Notify() => Notify(Void.data);
    }
}