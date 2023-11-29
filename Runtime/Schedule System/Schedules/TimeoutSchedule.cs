using System;

namespace Lab5Games.Schedules
{
    public class TimeoutSchedule : BaseSchedules, IAwaiter, IAwaitable<TimeoutSchedule>
    {
        bool _isCompleted;
        float _secondsTimeout;
               
        public float RemainingTime { get; private set; }

        public TimeoutSchedule(float timeout)
        {
            _isCompleted = false;
            SetTimeout(timeout);
        }

        public void SetTimeout(float seconds)
        {
            _secondsTimeout = seconds;
        }

        protected override void OnStart()
        {
            _isCompleted = false;
            RemainingTime = _secondsTimeout;
        }

        protected override void OnComplete()
        {
            _isCompleted = true;
            RemainingTime = 0;
        }

        protected override void OnCancel()
        {
            _isCompleted = true;
        }

        protected override void OnUpdate(float deltaTime)
        {
            RemainingTime -= deltaTime;

            if(RemainingTime <= 0)
            {
                Complete();
            }
        }

        public bool IsCompleted => _isCompleted;

        public void GetResult()
        {
            
        }

        public void OnCompleted(Action continuation)
        {
            scheduleCompleted += x => continuation();
            scheduleCanceled += x => continuation();
        }

        public TimeoutSchedule GetAwaiter() => this;
    }
}
