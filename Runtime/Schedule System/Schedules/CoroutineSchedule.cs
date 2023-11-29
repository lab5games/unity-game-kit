using System.Collections;
using UnityEngine;

namespace Lab5Games.Schedules
{
    public class CoroutineSchedule : BaseSchedules
    {
        IEnumerator _routine;
        Coroutine _coroutine;
        bool _running;

        public CoroutineSchedule(IEnumerator routine)
        {
            _routine = routine;
        }

        protected override void OnStart()
        {
            _running = true;
            _coroutine = ScheduleManager.Instance.StartCoroutine(Routine());
        }

        protected override void OnComplete()
        {
            _running = false;
        }

        protected override void OnCancel()
        {
            if(_coroutine != null)
            {
                ScheduleManager.Instance.StopCoroutine(_coroutine);
            }

            _coroutine = null;
            _running = false;
        }

        IEnumerator Routine()
        {
            while(_running)
            {
                if (_routine.MoveNext())
                {
                    yield return null;
                }
                else
                {
                    Complete();
                }
            }
        }
    }
}