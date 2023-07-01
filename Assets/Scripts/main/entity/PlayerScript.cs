using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using TMPro;

/// <author> Rohaid & Ben </author> 
public class PlayerScript : LoggableMonoBehaviour {

    [Header("Colliders")]
    public BoxCollider2D SuccessCollider;
    public ContactFilter2D contactFilter;
    private Collider2D[] successColliderContactResults = new Collider2D[20];

    [Header("Input System")]
    // Input Controller
    private PlayerInput input;

    [Header("Cool down")]
    [SerializeField] private float COOLDOWN_SECONDS = 1.0f;
    private bool isCoolDown = false;
    

    [Header("Debug Mode")]
    [SerializeField] private TextMeshProUGUI debugText;

    void Awake() {
        Assert.IsNotNull(SuccessCollider);
        Assert.IsFalse(COOLDOWN_SECONDS < 0f);

        input = new PlayerInput();
        input.Enable();
        input.Player.Enable();
        input.Player.UP.Enable();
        input.Player.DOWN.Enable();
        input.Player.RIGHT.Enable();
    }

    void Update(){
        if(DebugMode){
            Assert.IsNotNull(debugText);   
            if(isCoolDown){
                debugText.text = "Can't Move";
                debugText.color = Color.red;
            } else {
                debugText.text = "Can Move";
                debugText.color = Color.green;
            }
        }
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

    void OnUP(){
        Act(ActionEnum.UP);
    }

    void OnDOWN(){
        Act(ActionEnum.DOWN);
    }

    void OnRIGHT(){
        Act(ActionEnum.RIGHT);
    }

    void OnCoolDown(){
        isCoolDown = false; 
    }

    ///<summary> Detects objects and if the current input was used </summary>
    public void Act(ActionEnum dir) {
        if(!isCoolDown){
            isCoolDown = true; 
            Invoke("OnCoolDown",COOLDOWN_SECONDS);
        } else {
            return;
        }

        int countOfSuccessCollisions = SuccessCollider.OverlapCollider(contactFilter, successColliderContactResults);
        if (countOfSuccessCollisions > 0) {
        for (int i = 0; i<countOfSuccessCollisions; i++) {
            Log($"player success collision with: {successColliderContactResults[i].transform.name}");
            // TODO: if the obstacle in the collision was the right one for the key pressed, trigger that obstacle to change states
            ObstacleAScript scrip = successColliderContactResults[i].gameObject.GetComponentInChildren<ObstacleAScript>();
            if(scrip.getActionType().dir == dir){
                scrip.changeState(ObstacleAScript.STATE.PlayerResolvedSuccessfully);
            }
                }
            }
    } 

}

