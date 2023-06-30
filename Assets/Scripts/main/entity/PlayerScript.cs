using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

// author : Rohaid 
public class PlayerScript : MonoBehaviour {

    public BoxCollider2D SuccessCollider;

    // for detecting failure collisions
    public BoxCollider2D FailureCollider;
    public ContactFilter2D contactFilter;
    private Collider2D[] failureColliderContactResults = new Collider2D[20];
    private Collider2D[] successColliderContactResults = new Collider2D[20];

    // Input 
    private PlayerInput input;

    void Awake() {
        Assert.IsNotNull(FailureCollider);
        Assert.IsNotNull(SuccessCollider);

        input = new PlayerInput();
        input.Enable();
        input.Player.Enable();
        input.Player.UP.Enable();
        input.Player.DOWN.Enable();
        input.Player.RIGHT.Enable();
    }

    void onEnable() {
        input.Enable();
        input.Player.Enable();
        input.Player.UP.Enable();
        input.Player.DOWN.Enable();
        input.Player.RIGHT.Enable();
    }

    void OnDisable() {
        input.Player.UP.Disable();
        input.Player.DOWN.Disable();
        input.Player.RIGHT.Disable();
        input.Player.Disable();
        input.Disable(); 
    }

    public void Update() {
      
    }

    void OnUP(){
        Act();
    }

    void OnDOWN(){
        Act();
    }

    void OnRIGHT(){
        Act();
    }

    public void Act() {
          // check for any success collisions
            Debug.Log("Pressed Button");
            int countOfSuccessCollisions = SuccessCollider.OverlapCollider(contactFilter, successColliderContactResults);
            if (countOfSuccessCollisions > 0) {
                for (int i = 0; i<countOfSuccessCollisions; i++) {
                    Debug.Log($"player success collision with: {successColliderContactResults[i].transform.name}");
                    // TODO: if the obstacle in the collision was the right one for the key pressed, trigger that obstacle to change states
                    successColliderContactResults[i].gameObject.GetComponentInChildren<ObstacleAScript>().changeState(ObstacleAScript.STATE.PlayerResolvedSuccessfully);
                }
            }
    } 

}

// Checks that an object touched the back collider. Should be moved to an OnTrigger Enter script though instead
// the update loop ?

//  // check for any failure collisions

//         int countOfFailureCollisions = FailureCollider.OverlapCollider(contactFilter, failureColliderContactResults);
//         if (countOfFailureCollisions > 0) {
//             for(int i = 0; i<countOfFailureCollisions; i++) {
//                 Debug.Log($"player failure collision with: {failureColliderContactResults[i].transform.name}");
//                 // TODO: handle collision: lose the game, etc.
//                 this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
//                 failureColliderContactResults[i].gameObject.GetComponentInChildren<ObstacleAScript>().changeState(ObstacleAScript.STATE.PlayerFailed);
//             }
//         }
