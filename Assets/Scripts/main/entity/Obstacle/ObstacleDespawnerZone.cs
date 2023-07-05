using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObstacleDespawnerZone : MonoBehaviour
{
    public BoxCollider2D Collider;
    private Collider2D[] ColliderContactResults = new Collider2D[20];
    public ContactFilter2D contactFilter;

    public bool DebugMode;

    void Awake() {
        Assert.IsNotNull(Collider);
    }

    void Update() {

        int countOfCollisions = Collider.OverlapCollider(contactFilter, ColliderContactResults);
        if (countOfCollisions == 0)
            return;

        for(int i=0; i<countOfCollisions; i++) {
            Collider2D collision = ColliderContactResults[i];
            if(DebugMode) Debug.Log($"Obstacle Despawner Zone collision with: {collision.transform.name}");

            // if collision was with an obstacle, tell it to despawn itself
            ObstacleAScript obstacleAScript = collision.gameObject.GetComponentInChildren<ObstacleAScript>();
            if(obstacleAScript != null) {
                obstacleAScript.HandleCollisionWithDespawnerZone();
            }

            // if powerup hits the despawner zone, tell it to despawn itself
            PowerupAScript powerupAScript = collision.gameObject.GetComponentInChildren<PowerupAScript>();
            if (powerupAScript != null) {
                powerupAScript.DespawnPowerup();
            }
        }

    }

}
