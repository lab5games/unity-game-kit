
namespace Lab5Games.Schedules
{
    public delegate void ScheduleCallback(BaseSchedules schedule);

    public abstract class BaseSchedules
    {
        public enum EState
        {
            InProgress,
            Completed,
            Canceled
        }


        public EState State { get; private set; } = EState.InProgress;

        public event ScheduleCallback scheduleCompleted;
        public event ScheduleCallback scheduleCanceled;

        protected virtual void OnStart() { }
        protected virtual void OnComplete() { }
        protected virtual void OnCancel() { }
        protected virtual void OnUpdate(float deltaTime) { }

        public void Start()
        {
            if(State == EState.InProgress)
            {
                UnityEngine.Debug.LogError($"The schedule {GetType().Name} is still in progress");
                return;
            }
            else
            {
                if(!ScheduleManager.AddSchedule(this))
                {
                    UnityEngine.Debug.LogError($"Failed to add this schedule {GetType().Name} to the schedule manager");
                }
            }
        }

        public void Complete()
        {
            if(State == EState.InProgress)
            {
                State = EState.Completed;
            }
        }

        public void Cancel()
        {
            if(State == EState.InProgress)
            {
                State = EState.Canceled;
            }
        }

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        internal void BeforeStartSchedule()
        {
            OnStart();
            State = EState.InProgress;
        }

        internal void BeforeEndSchedule()
        {
            if(State == EState.Completed)
            {
                OnComplete();
                scheduleCompleted?.Invoke(this);
            }
            else if(State == EState.Canceled)
            {
                OnCancel();
                scheduleCanceled?.Invoke(this);
            }
        }
    }
}
