using UnityEngine;
using ObstacleManagement;
using UnityEngine.Assertions;

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
        public const float MAXIMUM_SPAWN_RATE = 45.0f;

        /// <summary>
        /// Determines the minimium amount of seconds that the timer will run.
        /// </summary>
        public const float MINIMUM_SPAWN_RATE = 30.0f;

        public const float POWERUP_LENGTH = 7.0f;

        /// <summary>
        /// The timer used to wait a certain amount of time before generating obstacles
        /// </summary>
        [SerializeField] private Timer2 _timer;

        private bool isPowerup = false;

        [SerializeField] private GameObject powerUpIndicator; 

        /// <summary>
        /// Sets up this manager's singleton and creates the powerup timer
        /// </summary>
        private void Awake()
        {
            UnityEngine.Assertions.Assert.IsNull(
                Instance,
                $"A singleton instance must be null. Is there another class in the scene? Type: {GetType()}"
            );
            Instance = this;
            _timer = new Timer2(MAXIMUM_SPAWN_RATE);

            Assert.IsNotNull(powerUpIndicator);
            powerUpIndicator.SetActive(false);
        }

        private void Update()
        {
            if (GameStateManager.instance.CurrentState != GameStateManager.STATE.PLAYING)
                return;
            // this ticks the timer
            bool timerAlerted = _timer.UpdateTimerProgress(Time.deltaTime);
            if (timerAlerted) {
                NotifyOfPowerupTimerEnd();
            } 
        }

        public void startPowerup(){
            isPowerup = true;
            powerUpIndicator.SetActive(true);
            _timer.UpdateGameTimeBetweenAlerts(POWERUP_LENGTH);
            _timer.ResetRemainingTimeToFullAmount();
        }

        public bool getPowerupState(){
            return isPowerup;
        }

        /// <summary>
        /// The PowerupTimer calls this method when it has ended.
        /// When notified, an obstacle is spawned using the PowerupSpawnerScript,
        /// the spawn rate is increased (if possible), and the timer is restarted.
        /// </summary>
        public void NotifyOfPowerupTimerEnd()
        {
            if(isPowerup){
                isPowerup = false;
                powerUpIndicator.SetActive(false);
                RandomizeSpawnRate();
                _timer.ResetRemainingTimeToFullAmount();
            } else {
                Log("The PowerupTimer has notified the PowerupManager that the timer has ended");

                ObstacleManager.Instance.SpawnAPowerUpInsteadOfObstacleNextTime();
                Log("Told the PowerupSpawnerScript to spawn an obstacle");

                RandomizeSpawnRate();
                Log("Increased the powerup spawn rate, if possible");

                _timer.ResetRemainingTimeToFullAmount();
                Log("Restarted the PowerupTimer");
            }
            
        }

        /// <summary>
        /// When the game is started, the timer for the powerups should be started
        /// </summary>
        public void OnGameStart()
        {
            isPowerup = false;
            _timer.UpdateGameTimeBetweenAlerts(MAXIMUM_SPAWN_RATE);
            _timer.ResetRemainingTimeToFullAmount();
            Log("Started the PowerupTimer");
        }

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
