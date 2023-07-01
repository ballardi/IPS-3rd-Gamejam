using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace ObstacleManagement
{
    public class ObstacleTimer
    {
        /// <summary>
        /// Determines the amount of seconds that the timer will wait initially.
        /// This initial spawn rate will be increased after each obstacle generation.
        /// </summary>
        public const float INITIAL_SPAWN_RATE = 1.2f;

        /// <summary>
        /// Determines the amount of seconds that should be decreased from the timer
        /// after each generated obstacle.
        /// Since the timer will end sooner, this makes the game more difficult.
        /// The spawn rate should not be decremented if the minimum spawn rate has been reached.
        /// </summary>
        public const float SPAWN_RATE_DECREMENT = 0.03f;

        /// <summary>
        /// Determines the minimium amount of seconds that the timer will run.
        /// After this rate has been reached, it cannot be decremented any more.
        /// </summary>
        public const float MINIMUM_SPAWN_RATE = 0.3f;

        /// <summary>
        /// The current spawn rate determining the amount of seconds on the timer.
        /// The spawn rate should be decreased after each generated obstacle.
        /// </summary>
        private float _spawnRate = INITIAL_SPAWN_RATE;

        /// <summary>
        /// The obstacle manager script, which is a MonoBehaviour.
        /// This is used to:
        /// - access its StartCoroutine method to push control to the manager
        /// - alert it when the timer has ended
        /// </summary>
        private readonly ObstacleManager _parent;

        /// <summary>
        /// Used to track if there is a timer running at the moment.
        /// If a timer is running, the system should not try to start it.
        /// </summary>
        private Coroutine _clock;

        /// <summary>
        /// Creates the ObstacleTimer, sets its spawn rate to the default spawn rate,
        /// and uses the specified obstacle manager script as the parent.
        /// </summary>
        /// <param name="parent">The non-null reference to the ObstacleManager script</param>
        public ObstacleTimer(ObstacleManager parent)
        {
            Assert.IsNotNull(parent);
            _parent = parent;
        }

        /// <summary>
        /// Starts the timer if it is not already running.
        /// Uses the current spawn rate as the duration of the timer in seconds.
        /// If the timer is running, an Exception is thrown.
        /// </summary>
        public void Start()
        {
            Assert.IsNull(_clock, "Should not start a timer if it is already running!");
            _clock = _parent.StartCoroutine(Tick());
        }

        /// <summary>
        /// Decreases the amount of seconds on the timer, therefore increasing the spawn rate.
        /// If this would be less than the minimum spawn rate, the spawn rate will be set to the minimum.
        /// </summary>
        public void IncreaseSpawnRate()
        {
            _spawnRate = Mathf.Clamp(
                _spawnRate - SPAWN_RATE_DECREMENT,
                MINIMUM_SPAWN_RATE,
                INITIAL_SPAWN_RATE
            );
        }

        /// <summary>
        /// Waits until the amount of seconds of the current spawn rate have passed,
        /// stops the timer, and then notifies the ObstacleManager that the timer has ended.
        /// </summary>
        /// <returns>An IEnumerator in order to wait for the spawn rate to end</returns>
        private IEnumerator Tick()
        {
            yield return new WaitForSeconds(_spawnRate);
            _clock = null;
            _parent.NotifyOfObstacleTimerEnd();
        }
    }
}
