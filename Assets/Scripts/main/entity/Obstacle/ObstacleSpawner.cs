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
        /// A reference to the spawn point for obstacles.
        /// </summary>
        [SerializeField] private Transform _obstacleASpointPoint;
        [SerializeField] private Transform _obstacleBSpointPoint;
        [SerializeField] private Transform _obstacleCSpointPoint;
        [SerializeField] private int[] previousObstacles;

        public ObjectPool poolA, poolB, poolC;

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

            Assert.IsNotNull(poolA);
            Assert.IsNotNull(poolB);
            Assert.IsNotNull(poolC);

            Assert.IsNotNull(_obstacleASpointPoint);
            Assert.IsNotNull(_obstacleBSpointPoint);
            Assert.IsNotNull(_obstacleCSpointPoint);

            previousObstacles =  new int[3];
            previousObstacles[0] = -1;
            previousObstacles[1] = -1;
            previousObstacles[2] = -1;
        }

        /// <summary>
        /// Selects a random obstacle from the Prefab array and spawns it
        /// </summary>
        public void SpawnObstacle()
        {
            int randomObstacleType = Random.Range(0, 3);
            // O is Up
            // 1 is Right
            // 2 is down
            
            bool fourthSameObstacle = true;
            while (fourthSameObstacle){
                if((randomObstacleType == previousObstacles[0] && 
                randomObstacleType == previousObstacles[1] )&& 
                randomObstacleType == previousObstacles[2] ){
                    randomObstacleType = Random.Range(0,3);
                } else {
                    fourthSameObstacle = false;
                }
            }
            previousObstacles[0] = previousObstacles[1];
            previousObstacles[1] = previousObstacles[2];
            previousObstacles[2] = randomObstacleType;
            
            

            GameObject obstacleCreated = null;
            switch (randomObstacleType) {
                case 0: obstacleCreated = poolA.RetrieveAvailableObject(); break;
                case 1: obstacleCreated = poolB.RetrieveAvailableObject(); break;
                case 2: obstacleCreated = poolC.RetrieveAvailableObject(); break;
                default: throw new System.Exception("should never happen");
            }

            switch (randomObstacleType) {
                case 0: obstacleCreated.transform.position = _obstacleASpointPoint.position; break;
                case 1: obstacleCreated.transform.position = _obstacleBSpointPoint.position; break;
                case 2: obstacleCreated.transform.position = _obstacleCSpointPoint.position; break;
                default: throw new System.Exception("should never happen");
            }

            obstacleCreated.SetActive(true);
        }
    }
}
