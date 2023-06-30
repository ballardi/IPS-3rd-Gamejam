namespace ObstacleManagement
{
    public class ObstacleManager : LoggableMonoBehaviour
    {
        /// <summary>
        /// The instance of the manager's singleton
        /// </summary>
        /// <value></value>
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

            _obstacleTimer.Start();
            Log("Started the ObstacleTimer");
        }

        public void NotifyOfObstacleTimerEnd()
        {
            Log("The ObstacleTimer has notified the ObstacleManager that the timer has ended");

            // Spawn obstacle
        }
    }
}
