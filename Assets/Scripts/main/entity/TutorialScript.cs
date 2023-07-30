using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Tutorial : LoggableMonoBehaviour
{
     // for detecting failure collisions
    public BoxCollider2D Collider;
    private Collider2D[] collisions = new Collider2D[20];
    public ContactFilter2D contactFilter;

    public PlayerScript playerScript;

    void Awake() {
        Assert.IsNotNull(playerScript);
        Assert.IsNotNull(Collider);
    }

    
    // Checks that an object touched the collider.
    void Update() {
        // don't check for collisions unless we're actively playing the game
        if (GameStateManager.instance.CurrentState != GameStateManager.STATE.PLAYING)
            return;

        // don't check for collisions if all the tutorials have been shown before
        // (just a performance optimization)
        if (GameStateManager.instance.HasPlayerSeenAllActionTutorials())
            return;

        int collisionCount = Collider.OverlapCollider(contactFilter, collisions);
        if (collisionCount == 0)
            return;

        for (int i = 0; i<collisionCount; i++) {
            ObstacleAScript obstacle = collisions[i].GetComponentInChildren<ObstacleAScript>();

            Log($"tutorial collision with {obstacle.name} (instanceid: {obstacle.GetInstanceID()}) (state: {obstacle.GetCurrentState()}) (collider enabled: {obstacle.GetComponent<BoxCollider2D>().enabled})");
            obstacle.HandleSuccessCollisionInProgress();
        }
    }
        
}
