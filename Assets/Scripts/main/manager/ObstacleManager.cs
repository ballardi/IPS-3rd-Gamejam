namespace ObstacleManagement
{
    /// <summary>
    /// The ObstacleManager's single responsibility is managing the generation of obstacles.
    /// It uses a custom timer as a generation interval.
    /// </summary>
    public class ObstacleManager : LoggableMonoBehaviour
    {
        /// <summary>
        /// The instance of the manager's singleton
        /// </summary>
        public static ObstacleManager Instance { private set; get; }

        /// <summary>
        /// The timer used to wait a certain amount of time before generating obstacles
        /// </summary>
        private ObstacleTimer _obstacleTimer;

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
            Log("Created the ObstacleManager singleton");

            _obstacleTimer = new ObstacleTimer(this);
            Log("Created the ObstacleTimer");
        }

        /// <summary>
        /// The ObstacleTimer calls this method when it has ended.
        /// When notified, an obstacle is spawned using the ObstacleSpawnerScript,
        /// the spawn rate is increased (if possible), and the timer is restarted.
        /// </summary>
        public void NotifyOfObstacleTimerEnd()
        {
            Log("The ObstacleTimer has notified the ObstacleManager that the timer has ended");

            ObstacleSpawnerScript.Instance.SpawnObstacle();
            Log("Told the ObstacleSpawnerScript to spawn an obstacle");

            _obstacleTimer.IncreaseSpawnRate();
            Log("Increased the obstacle spawn rate, if possible");

            _obstacleTimer.Start();
            Log("Restarted the ObstacleTimer");
        }

        /// <summary>
        /// When the game is started, the timer for the obstacles should be started
        /// </summary>
        public void OnGameStart()
        {
            _obstacleTimer.Start();
            Log("Started the ObstacleTimer");
        }

        /// <summary>
        /// When the game is ended, the timer should be stopped
        /// </summary>
        public void OnGameEnd()
        {
            _obstacleTimer.Stop();
            Log("Stopped the ObstacleTimer");
        }
    }
}
