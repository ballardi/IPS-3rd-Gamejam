using UnityEngine;

namespace PowerupManagement
{
    /// <summary>
    /// The ObstacleTimer class is a timer responsible for providing an obstacle generation interval.
    /// Its timer duration is decreased each time an obstacle is generated until
    /// the minimum duration has been reached.
    /// </summary>
    public class PowerupTimer : Timer
    {
        /// <summary>
        /// Determines the maximum amount of seconds that the timer will run.
        /// </summary>
        public const float MAXIMUM_SPAWN_RATE = 35.0f;

        /// <summary>
        /// Determines the minimium amount of seconds that the timer will run.
        /// </summary>
        public const float MINIMUM_SPAWN_RATE = 45.0f;

        private bool isPowerup = false;

        /// <summary>
        /// Creates the PowerupTimer, sets its spawn rate duration to the default spawn rate,
        /// and uses the specified obstacle manager script as the parent.
        /// </summary>
        /// <param name="parent">The non-null reference to the PowerupManager script</param>
        public PowerupTimer(PowerupManager parent)
            : base(parent)
        {
            _duration = MAXIMUM_SPAWN_RATE * 60.0f;
        }

        /// <summary>
        /// Randomizes the amount of seconds on the timer.
        /// If this would be less than the minimum spawn rate, the spawn rate will be set to the minimum.
        /// </summary>
        public void RandomizeSpawnRate()
        {
            _duration = Random.Range(MINIMUM_SPAWN_RATE, MAXIMUM_SPAWN_RATE) * 60.0f;
        }

        /// <summary>
        /// Sets the duration of the powerup timer
        /// </summary>

        public void PauseSpawnRate(float PowerupLength)
        {
            isPowerup = true;
            _duration = Mathf.Clamp(PowerupLength, 0.0f, PowerupLength) * 60.0f; // 60 F is to account for frames
        }

        public bool powerupState(){
            return isPowerup;
        }

        /// <summary>
        /// When the timer has ended, it notifies the PowerupManager.
        /// </summary>
        protected override void Alert()
        {
            if(!isPowerup){
                PowerupManager.Instance.NotifyOfPowerupTimerEnd();
            } else {
                isPowerup = false;
                RandomizeSpawnRate();
                StartTimer();
            }
           
        }
    }
}
