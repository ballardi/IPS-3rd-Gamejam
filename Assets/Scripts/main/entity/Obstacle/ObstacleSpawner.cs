namespace ObstacleManagement
{
    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    /// A class used to spawn obstacles.
    /// Note that only the ObstacleManager class should spawn obstacles.
    /// </summary>
    /// <author>Gino</author>
    public class ObstacleSpawner : LoggableMonoBehaviour
    {
        /// <summary>
        /// The instance of the spawner's singleton
        /// </summary>
        public static ObstacleSpawner Instance { private set; get; }

        /// <summary>
        /// A reference of all obstacle prefabs that need to be
        /// edited in the Inspector
        /// </summary>
        [SerializeField]
        private GameObject[] _obstaclePrefabs;

        /// <summary>
        /// A reference to the spawn point for all obstacles.
        /// Note that the height needs to be adjusted within the obstacle prefab,
        /// if it should spawn higher or lower than the default spawn point.
        /// </summary>
        [SerializeField]
        private Transform _obstacleSpointPoint;

        /// <summary>
        /// Sets up the spawner's singleton instance
        /// </summary>
        private void Awake()
        {
            Assert.IsNull(
                Instance,
                $"A singleton instance must be null. Is there another class in the scene? Type: {GetType()}"
            );
            Instance = this;
            Log("Created the ObstacleManager singleton");

            Assert.IsNotNull(_obstacleSpointPoint);
            Assert.IsNotNull(_obstacleSpointPoint);
        }

        /// <summary>
        /// Selects a random obstacle from the Prefab array and spawns it
        /// </summary>
        public void SpawnObstacle()
        {
            var randomObstacle = _obstaclePrefabs[Random.Range(0, _obstaclePrefabs.Length)];

            // TODO: Pool game objects
            Instantiate(
                randomObstacle,
                _obstacleSpointPoint.position,
                Quaternion.identity,
                transform
            );
        }
    }
}
