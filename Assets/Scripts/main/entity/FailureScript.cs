using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FailureScript : MonoBehaviour
{
     // for detecting failure collisions
    public BoxCollider2D FailureCollider;
    private Collider2D[] failureColliderContactResults = new Collider2D[20];
    public ContactFilter2D contactFilter;

    void Awake() {
        Assert.IsNotNull(FailureCollider);
    }

    
    // Checks that an object touched the back collider.
    void Update() {
        // don't check for collisions unless we're actively playing the game
        if (GameStateManager.instance.CurrentState != GameStateManager.STATE.PLAYING)
            return;

        int countOfFailureCollisions = FailureCollider.OverlapCollider(contactFilter, failureColliderContactResults);
            if (countOfFailureCollisions > 0) {
                Collider2D firstCollisionInList = failureColliderContactResults[0];
                Debug.Log($"player failure collision with: {firstCollisionInList.transform.name}");
                // TODO: handle collision: lose the game, etc.
                this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                firstCollisionInList.gameObject.GetComponentInChildren<ObstacleAScript>().changeState(ObstacleAScript.STATE.PlayerFailed);
                GameStateManager.instance.OnGameOver();
                return; // return since it's already gameover and don't want to transition state to gameover multiple times
            }
    }
        
}
