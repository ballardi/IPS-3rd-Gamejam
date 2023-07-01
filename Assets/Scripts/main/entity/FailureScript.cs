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
    int countOfFailureCollisions = FailureCollider.OverlapCollider(contactFilter, failureColliderContactResults);
        if (countOfFailureCollisions > 0) {
            for(int i = 0; i<countOfFailureCollisions; i++) {
                Debug.Log($"player failure collision with: {failureColliderContactResults[i].transform.name}");
                // TODO: handle collision: lose the game, etc.
                this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                failureColliderContactResults[i].gameObject.GetComponentInChildren<ObstacleAScript>().changeState(ObstacleAScript.STATE.PlayerFailed);
            }
        }
}
        
}
