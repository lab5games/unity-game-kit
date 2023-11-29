using System.Collections.Generic;
using UnityEngine;

namespace Lab5Games.Schedules
{
    public class ScheduleManager : Singleton<ScheduleManager>
    {
        List<BaseSchedules> _scheduleList = new List<BaseSchedules>();

        internal static bool AddSchedule(BaseSchedules schedule)
        {
            if(Instance._scheduleList.AddUnique(schedule))
            {
                schedule.BeforeStartSchedule();
                return true;
            }

            return false;
        }

        #region Unity Calls
        private void FixedUpdate()
        {
            float dt = Time.deltaTime;

            for(int i=_scheduleList.Count-1; i>=0; i--)
            {
                var schedule = _scheduleList[i];

                switch(schedule.State)
                {
                    case BaseSchedules.EState.InProgress:
                        schedule.Update(dt);
                        break;

                    case BaseSchedules.EState.Completed:
                    case BaseSchedules.EState.Canceled:
                        _scheduleList.RemoveAt(i);
                        schedule.BeforeEndSchedule();
                        break;
                }
            }
        }
        #endregion
    }
}