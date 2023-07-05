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

        public GameObject UPPrefab, RIGHTPrefab, DOWNPrefab;

        public Transform UPLocation, RIGHTLocation, DOWNLocation;

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
            Log("Created the PowerupSpawner singleton");

            Assert.IsNotNull(UPPrefab);
            Assert.IsNotNull(DOWNPrefab);
            Assert.IsNotNull(RIGHTPrefab);
            Assert.IsNotNull(UPLocation);
            Assert.IsNotNull(DOWNLocation);
            Assert.IsNotNull(RIGHTLocation);
        }

        /// <update> Generates a powerup at a random location </update>

        public void SpawnPowerup()
        {
            int randomObstacleType = Random.Range(0, 3);
            
            // O is Up
            // 1 is Right
            // 2 is Down
            Vector3 pos = new Vector3(0f,0f,0f);

            switch (randomObstacleType) {
                case 0: pos = UPLocation.position; break;
                case 1: pos = RIGHTLocation.position; break;
                case 2: pos = DOWNLocation.position; break;
                default: throw new System.Exception("should never happen");
            }

            switch(randomObstacleType) {
                case 0:
                    Instantiate(UPPrefab, pos, Quaternion.identity, transform);
                    break;
                case 1: 
                    Instantiate(RIGHTPrefab, pos, Quaternion.identity, transform);
                    break;
                case 2:
                    Instantiate(DOWNPrefab, pos, Quaternion.identity, transform);
                    break;
                default:
                    throw new System.Exception("should never happen");
            }
        }
    }
}
