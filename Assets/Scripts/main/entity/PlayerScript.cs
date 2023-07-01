using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

/// <author> Rohaid & Ben </author> 
public class PlayerScript : MonoBehaviour {

    public BoxCollider2D SuccessCollider;
    public ContactFilter2D contactFilter;
    private Collider2D[] successColliderContactResults = new Collider2D[20];

    // Input Controller
    private PlayerInput input;

    void Awake() {
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
        Act(ActionEnum.UP);
    }

    void OnDOWN(){
        Act(ActionEnum.DOWN);
    }

    void OnRIGHT(){
        Act(ActionEnum.RIGHT);
    }

    ///<summary> Detects objects and if the current input was used </summary>
    public void Act(ActionEnum dir) {
        int countOfSuccessCollisions = SuccessCollider.OverlapCollider(contactFilter, successColliderContactResults);
        if (countOfSuccessCollisions > 0) {
        for (int i = 0; i<countOfSuccessCollisions; i++) {
            Debug.Log($"player success collision with: {successColliderContactResults[i].transform.name}");
            // TODO: if the obstacle in the collision was the right one for the key pressed, trigger that obstacle to change states
            ObstacleAScript scrip = successColliderContactResults[i].gameObject.GetComponentInChildren<ObstacleAScript>();
            if(scrip.getActionType().dir == dir){
                scrip.changeState(ObstacleAScript.STATE.PlayerResolvedSuccessfully);
            }
                }
            }
    } 

}

