using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FailureScript : LoggableMonoBehaviour
{
     // for detecting failure collisions
    public BoxCollider2D FailureCollider;
    private Collider2D[] collisions = new Collider2D[20];
    public ContactFilter2D contactFilter;

    public PlayerScript playerScript;

    void Awake() {
        Assert.IsNotNull(playerScript);
        Assert.IsNotNull(FailureCollider);
    }

    
    // Checks that an object touched the back collider.
    void Update() {
        // don't check for collisions unless we're actively playing the game
        if (GameStateManager.instance.CurrentState != GameStateManager.STATE.PLAYING)
            return;

        int collisionCount = FailureCollider.OverlapCollider(contactFilter, collisions);
        if (collisionCount == 0)
            return;

        for (int i = 0; i<collisionCount; i++) {
            ObstacleAScript obstacle = collisions[i].GetComponentInChildren<ObstacleAScript>();

            // skip collisions unless the obstacle is in normal state
            if (obstacle.GetCurrentState() != ObstacleAScript.STATE.Normal)
                continue;

            Log($"failure collision with {obstacle.name} (instanceid: {obstacle.GetInstanceID()}) (state: {obstacle.GetCurrentState()}) (collider enabled: {obstacle.GetComponent<BoxCollider2D>().enabled})");
            obstacle.HandlePlayerFailedBecauseOfThisObstacle();
            playerScript.OnFailure();
            GameStateManager.instance.OnGameOver();
            return; // return since it's already gameover and don't want to transition state to gameover multiple times
        }
    }
        
}
