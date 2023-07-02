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
        /// The timer used to wait a certain amount of time before generating obstacles
        /// </summary>
        private PowerupTimer _powerupTimer;

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
            Log("Created the PowerupManager singleton");

            _powerupTimer = new PowerupTimer(this);
            Log("Created the PowerupTimer");

            _powerupTimer.Start();
            Log("Started the PowerupTimer");
        }

        /// <summary>
        /// The PowerupTimer calls this method when it has ended.
        /// When notified, an obstacle is spawned using the PowerupSpawnerScript,
        /// the spawn rate is increased (if possible), and the timer is restarted.
        /// </summary>
        public void NotifyOfObstacleTimerEnd()
        {
            Log("The PowerupTimer has notified the PowerupManager that the timer has ended");

            PowerupSpawnerScript.Instance.SpawnObstacle();
            Log("Told the PowerupSpawnerScript to spawn an obstacle");

            _powerupTimer.RandomizeSpawnRate();
            Log("Increased the powerup spawn rate, if possible");

            _powerupTimer.Start();
            Log("Restarted the PowerupTimer");
        }
    }
}
