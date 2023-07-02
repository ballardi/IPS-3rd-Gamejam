using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;

/// <author> Rohaid & Ben </author> 
public class PlayerScript : LoggableMonoBehaviour {

    public static PlayerScript instance;

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

    [Header("Animations")]
    public SpriteRenderer playerSpriteRenderer;
    public Animator animator;
    private const string TRIGGER_START_RUNNING = "StartRunning";
    private const string TRIGGER_START_UP      = "StartUp";
    private const string TRIGGER_START_RIGHT   = "StartRight";
    private const string TRIGGER_START_DOWN    = "StartDown";
    private const string TRIGGER_START_FAILURE = "StartFailure";

    [Header("Events")]
    public UnityEvent OnUpActionEvent;
    public UnityEvent OnRightActionEvent;
    public UnityEvent OnDownActionEvent;


    [Header("Debug Mode")]
    [SerializeField] private TextMeshProUGUI debugText;

    void Awake() {
        Assert.IsNull(instance); instance = this; // singleton set up

        Assert.IsNotNull(playerSpriteRenderer);
        Assert.IsNotNull(SuccessCollider);
        Assert.IsNotNull(animator);
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
        if(GameStateManager.instance.CurrentState != GameStateManager.STATE.PLAYING) {
            return;
        }

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

        // invoke events (used to trigger fmod sounds)
        switch (dir) {
            case ActionEnum.RIGHT: OnRightActionEvent.Invoke(); break;
            case ActionEnum.DOWN:  OnDownActionEvent.Invoke();  break;
            case ActionEnum.UP:    OnUpActionEvent.Invoke();    break;
        }

        // start the animation for the corresponding action
        ResetAllAnimationTriggers();
        switch (dir) {
            case ActionEnum.RIGHT: animator.SetTrigger(TRIGGER_START_RIGHT); break;
            case ActionEnum.DOWN:  animator.SetTrigger(TRIGGER_START_DOWN);  break;
            case ActionEnum.UP:    animator.SetTrigger(TRIGGER_START_UP);    break;
        }
    }

    public void OnNewGame() {
        playerSpriteRenderer.enabled = true;
        animator.SetTrigger(TRIGGER_START_RUNNING);
    }

    public void OnFailure() {
        animator.SetTrigger(TRIGGER_START_FAILURE);
    }

    private void ResetAllAnimationTriggers() {
        animator.ResetTrigger(TRIGGER_START_RUNNING);
        animator.ResetTrigger(TRIGGER_START_UP);
        animator.ResetTrigger(TRIGGER_START_RIGHT);
        animator.ResetTrigger(TRIGGER_START_DOWN);
        animator.ResetTrigger(TRIGGER_START_FAILURE);
    }

    public void OnStartTitleScreen() {
        playerSpriteRenderer.enabled = false;
    }

}

