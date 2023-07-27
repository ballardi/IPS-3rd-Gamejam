using PowerupManagement;
using UnityEngine;
namespace ObstacleManagement
{
    /// <summary>
    /// The ObstacleManager's single responsibility is managing the generation of obstacles.
    /// It uses a custom timer as a generation interval.
    /// </summary>
    public class ObstacleManager : LoggableMonoBehaviour {
        public static ObstacleManager Instance { private set; get; }

        /// Determines the amount of seconds that the timer will wait initially. This initial spawn rate duration will be decreased after each generated obstacle.
        public const float INITIAL_SPAWN_RATE = 1.6f;
        /// Determines the amount of seconds that should be decreased from the timer duration after each generated obstacle.
        /// The spawn rate should not be decremented if the minimum spawn rate has been reached.
        public const float SPAWN_RATE_DECREMENT = 0.012f;
        /// Determines the minimium amount of seconds that the timer will run. After this rate has been reached, it cannot be decreased any more.
        public const float MINIMUM_SPAWN_RATE = 0.640f;

        private Timer2 _timer;
        private bool shouldSpawnPowerupNextTime;

        private void Awake() {
            UnityEngine.Assertions.Assert.IsNull(Instance, $"A singleton instance must be null. Is there another class in the scene? Type: {GetType()}");
            Instance = this;
            _timer = new Timer2(INITIAL_SPAWN_RATE);
        }

        private void Update() {
            if (GameStateManager.instance.CurrentState != GameStateManager.STATE.PLAYING)
                return;

            bool timerAlerted = _timer.UpdateTimerProgress(Time.deltaTime);
            if (timerAlerted) {
                NotifyOfObstacleTimerEnd();
            }
        }

        /// <summary>
        /// The ObstacleTimer calls this method when it has ended.
        /// When notified, an obstacle is spawned using the ObstacleSpawnerScript,
        /// the spawn rate is increased (if possible), and the timer is restarted.
        /// </summary>
        public void NotifyOfObstacleTimerEnd()
        {
            if (shouldSpawnPowerupNextTime) {
                PowerupSpawnerScript.Instance.SpawnPowerup();
                shouldSpawnPowerupNextTime = false;
            } else {
                ObstacleSpawner.Instance.SpawnObstacle();
            }
                
            IncreaseSpawnRate();
            Log($"Obstacle spawned. Next timer duration: {_timer.GetTotalTimeBetweenAlerts()}");
        }

        /// <summary>
        /// Decreases the amount of seconds on the timer.
        /// If this would be less than the minimum spawn rate, the spawn rate will be set to the minimum.
        /// </summary>
        public void IncreaseSpawnRate()
        {
            float _newDuration = Mathf.Clamp(
                _timer.GetTotalTimeBetweenAlerts() - SPAWN_RATE_DECREMENT,
                MINIMUM_SPAWN_RATE,
                INITIAL_SPAWN_RATE
            );
            Log($"old timer duration: {_timer.GetTotalTimeBetweenAlerts()}. New timer duration: {_newDuration}");
            _timer.UpdateGameTimeBetweenAlerts(_newDuration);
        }

        /// <summary>
        /// When the game is started, the timer for the obstacles should be started
        /// </summary>
        public void OnGameStart() {
            _timer.UpdateGameTimeBetweenAlerts(INITIAL_SPAWN_RATE);
            _timer.ResetRemainingTimeToFullAmount();
            shouldSpawnPowerupNextTime = false;

            // spawn first obstacle immediately
            ObstacleSpawner.Instance.SpawnObstacle();

            Log("Started the ObstacleTimer and spawned a first obstacle");
        }

        public void SpawnAPowerUpInsteadOfObstacleNextTime() {
            shouldSpawnPowerupNextTime = true;
        }

    }
}
