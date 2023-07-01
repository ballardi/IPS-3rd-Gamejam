using UnityEngine;

namespace ObstacleManagement
{
    /// <summary>
    /// The ObstacleTimer class is a timer responsible for providing an obstacle generation interval.
    /// Its timer duration is decreased each time an obstacle is generated until
    /// the minimum duration has been reached.
    /// </summary>
    public class ObstacleTimer : Timer
    {
        /// <summary>
        /// Determines the amount of seconds that the timer will wait initially.
        /// This initial spawn rate duration will be decreased after each generated obstacle.
        /// </summary>
        public const float INITIAL_SPAWN_RATE = 1.2f;

        /// <summary>
        /// Determines the amount of seconds that should be decreased from the timer duration
        /// after each generated obstacle.
        /// The spawn rate should not be decremented if the minimum spawn rate has been reached.
        /// </summary>
        public const float SPAWN_RATE_DECREMENT = 0.03f;

        /// <summary>
        /// Determines the minimium amount of seconds that the timer will run.
        /// After this rate has been reached, it cannot be decreased any more.
        /// </summary>
        public const float MINIMUM_SPAWN_RATE = 0.3f;

        /// <summary>
        /// Creates the ObstacleTimer, sets its spawn rate duration to the default spawn rate,
        /// and uses the specified obstacle manager script as the parent.
        /// </summary>
        /// <param name="parent">The non-null reference to the ObstacleManager script</param>
        public ObstacleTimer(ObstacleManager parent)
            : base(parent)
        {
            _duration = INITIAL_SPAWN_RATE;
        }

        /// <summary>
        /// Decreases the amount of seconds on the timer.
        /// If this would be less than the minimum spawn rate, the spawn rate will be set to the minimum.
        /// </summary>
        public void IncreaseSpawnRate()
        {
            _duration = Mathf.Clamp(
                _duration - SPAWN_RATE_DECREMENT,
                MINIMUM_SPAWN_RATE,
                INITIAL_SPAWN_RATE
            );
        }

        /// <summary>
        /// When the timer has ended, it notifies the ObstacleManager.
        /// </summary>
        protected override void Alert()
        {
            (_parent as ObstacleManager).NotifyOfObstacleTimerEnd();
        }
    }
}
