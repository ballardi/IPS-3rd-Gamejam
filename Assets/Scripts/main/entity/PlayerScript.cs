using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;
using PowerupManagement;

/// <author> Rohaid & Ben </author> 
public class PlayerScript : LoggableMonoBehaviour {

    public static PlayerScript instance;

    [Header("Colliders")]
    public BoxCollider2D SuccessCollider;
    public ContactFilter2D contactFilter;
    private Collider2D[] collisions = new Collider2D[20];

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
    public UnityEvent OnFootstepEvent;


    [Header("Debug Mode")]
    [SerializeField] private TextMeshProUGUI debugText;

    [Header("Powerup State")]
    [SerializeField] private bool isPoweredup;

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
        // isPoweredup = PowerupManager.Instance.getPowerupState();
        if(isPoweredup){
            Act(ActionEnum.UP);
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

        int collisionCount = SuccessCollider.OverlapCollider(contactFilter, collisions);
        for (int i = 0; i<collisionCount; i++) {
            GameObject collisionObj = collisions[i].gameObject;
            Log($"player success collision {i} of {collisionCount}: {collisionObj.name} ({collisionObj.tag})");
           
            Log($"Is the player powered up? {isPoweredup}");
            switch (collisionObj.tag) {
                case "Obstacle":
                    ObstacleAScript obstacleScript = collisionObj.GetComponentInChildren<ObstacleAScript>();
                    // skip collisions unless the obstacle is in normal state
                    if (obstacleScript.GetCurrentState() != ObstacleAScript.STATE.Normal) {
                        Log($"player success collision skipped because obstacle in ignored state {obstacleScript.GetCurrentState()}, for: {obstacleScript.name}");
                        continue;
                    }
                    // skip collisions if the player used the wrong key
                    if (obstacleScript.getActionType().dir != dir && !isPoweredup) {
                        Log($"player success collision skipped because action {obstacleScript.getActionType().dir} different from pressed {dir}, for: {obstacleScript.name}");
                        continue;
                    }
                    Log($"player success collision with: {obstacleScript.name}");
                    obstacleScript.HandlePlayerResolvedThisObstacleSuccessfully();
                    break;

				case "Powerup" :
                    PowerupAScript powerupScript = collisionObj.GetComponentInChildren<PowerupAScript>();
                    // skip collisions if the player used the wrong key
                    if (powerupScript.getActionType().dir != dir) {
                        Log($"player success collision skipped because action {powerupScript.getActionType().dir} different from pressed {dir}, for: {powerupScript.name}");
                        continue;
                    }
                    Debug.Log($"Got the powerup");
					powerupScript.changeState(PowerupAScript.STATE.PlayerResolvedSuccessfully);
				break;

				default: 
                    throw new System.Exception("should never happen");
               
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
        animator.enabled = true;
        animator.speed = 1;
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

    public void HandlePlayerFootstep()
    {
        OnFootstepEvent.Invoke();
    }

    public void OnStartTitleScreen() {
        playerSpriteRenderer.enabled = false;
        animator.enabled = false;

    }

    public void OnPause() {
        animator.speed = 0;
    }

    public void OnUnpause() {
        animator.speed = 1;
    }

}

