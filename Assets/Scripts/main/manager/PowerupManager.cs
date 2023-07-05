using UnityEngine;
using ObstacleManagement;

namespace PowerupManagement
{
    /// <summary>
    /// The ObstacleManager's single responsibility is managing the generation of obstacles.
    /// It uses a custom timer as a generation interval.
    /// </summary>
    public class PowerupManager : LoggableMonoBehaviour
    {
        /// <summary>
        /// The instance of the manager's singleton
        /// </summary>
        public static PowerupManager Instance { private set; get; }

        /// <summary>
        /// Determines the maximum amount of seconds that the timer will run.
        /// </summary>
        public const float MAXIMUM_SPAWN_RATE = 5.0f;

        /// <summary>
        /// Determines the minimium amount of seconds that the timer will run.
        /// </summary>
        public const float MINIMUM_SPAWN_RATE = 3.0f;

        /// <summary>
        /// The timer used to wait a certain amount of time before generating obstacles
        /// </summary>
        [SerializeField] private Timer2 _timer;

        private bool isPowerup = false;

        /// <summary>
        /// Sets up this manager's singleton and creates the obstacle timer
        /// </summary>
        private void Awake()
        {
            UnityEngine.Assertions.Assert.IsNull(
                Instance,
                $"A singleton instance must be null. Is there another class in the scene? Type: {GetType()}"
            );
            Instance = this;
            _timer = new Timer2(MAXIMUM_SPAWN_RATE);
        }

        private void Update()
        {
            if (GameStateManager.instance.CurrentState != GameStateManager.STATE.PLAYING)
                return;
            
            bool timerAlerted = _timer.UpdateTimerProgress(Time.deltaTime);
            if (timerAlerted) {
                NotifyOfPowerupTimerEnd();
            } 
        }


        /// <summary>
        /// The PowerupAscript calls this method when a powerup has been successfully caught.
        /// When notified, the spawner timer is stopped and a timer is started 
        /// with the length of the powerup.
        /// </summary>
        // public void StartPowerupTimer(float timer) 
        // {
        //         Log("The PowerupTimer has notified the Powerup has started");
                
        //         _timer.Stop();
        //         Log("Stop current powerup timer started");

        //         _timer.PauseSpawnRate(timer);
        //         Log("Start the Powerup timer");

        //         _timer.StartTimer();

        // }

        /// <summary>
        /// The PowerupTimer calls this method when it has ended.
        /// When notified, an obstacle is spawned using the PowerupSpawnerScript,
        /// the spawn rate is increased (if possible), and the timer is restarted.
        /// </summary>
        public void NotifyOfPowerupTimerEnd()
        {
            Log("The PowerupTimer has notified the PowerupManager that the timer has ended");

            // PowerupSpawnerScript.Instance.SpawnPowerup();
            ObstacleManager.Instance.SpawnAPowerUpInsteadOfObstacleNextTime();
            Log("Told the PowerupSpawnerScript to spawn an obstacle");

            RandomizeSpawnRate();
            Log("Increased the powerup spawn rate, if possible");

            _timer.ResetRemainingTimeToFullAmount();
            Log("Restarted the PowerupTimer");
        }

        // public bool getPowerupState() {
        //     return _timer.powerupState();
        // }

        /// <summary>
        /// When the game is started, the timer for the powerups should be started
        /// </summary>
        public void OnGameStart()
        {
            _timer.UpdateGameTimeBetweenAlerts(MAXIMUM_SPAWN_RATE);
            _timer.ResetRemainingTimeToFullAmount();
            Log("Started the PowerupTimer");
        }

        /// <summary>
        /// When the game is ended, the timer should be stopped
        /// </summary>
        // public void OnGameEnd()
        // {
        //     _timer.Stop();
        //     Log("Stopped the PowerupTimer");
        // }

        /// <summary>
        /// Randomizes the amount of seconds on the timer.
        /// If this would be less than the minimum spawn rate, the spawn rate will be set to the minimum.
        /// </summary>
        public void RandomizeSpawnRate()
        {
            float _newDuration = Random.Range(MINIMUM_SPAWN_RATE, MAXIMUM_SPAWN_RATE);
            _timer.UpdateGameTimeBetweenAlerts(_newDuration);
        }
    }
}
