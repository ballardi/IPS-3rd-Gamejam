namespace PowerupManagement
{
    using UnityEngine;
    using UnityEngine.Assertions;

    public class PowerupSpawnerScript : LoggableMonoBehaviour
    {
        /// <summary>
        /// The instance of the spawner's singleton
        /// </summary>
        public static PowerupSpawnerScript Instance { private set; get; }

        public GameObject ObstacleAPrefab;

        public GameObject ObstacleASpawnPoint;

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

            Assert.IsNotNull(ObstacleAPrefab);
            Assert.IsNotNull(ObstacleASpawnPoint);
        }

        public void SpawnObstacle()
        {
            // TODO: Pool game objects
            Instantiate(
                ObstacleAPrefab,
                ObstacleASpawnPoint.transform.position,
                Quaternion.identity,
                transform
            );
        }
    }
}
