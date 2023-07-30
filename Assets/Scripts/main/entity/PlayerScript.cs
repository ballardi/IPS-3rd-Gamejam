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
    private const string TRIGGER_START_UP_SUCCESS = "StartUpSuccess";

    [Header("Events")]
    public UnityEvent OnUpActionEvent;
    public UnityEvent OnRightActionEvent;
    public UnityEvent OnDownActionEvent;
    public UnityEvent OnFootstepEvent;
    public UnityEvent OnObstacleResolution;
    public UnityEvent OnObstacleCollision;
    public UnityEvent OnPowerupResolution;

    [Header("Debug Mode")]
    [SerializeField] private TextMeshProUGUI debugText;

    [Header("Powerup State")]
    [SerializeField] private bool isPoweredup;

    [Header("Timing")]
    [SerializeField] private TimingScript timing;

    void Awake() {
        Assert.IsNull(instance); instance = this; // singleton set up

        Assert.IsNotNull(playerSpriteRenderer);
        Assert.IsNotNull(SuccessCollider);
        Assert.IsNotNull(animator);
        Assert.IsFalse(COOLDOWN_SECONDS < 0f);
        Assert.IsNotNull(timing, "The Timing Script is missing. Please Add");

        input = new PlayerInput();
        input.Enable();
        input.Player.Enable();
        input.Player.UP.Enable();
        input.Player.DOWN.Enable();
        input.Player.RIGHT.Enable();
    }

    void Update(){
        isPoweredup = PowerupManager.Instance.getPowerupState();
        if(isPoweredup){
            PowerupAct();
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
        if(!isPoweredup){
            Act(ActionEnum.UP);
        } 
    }

    void OnDOWN(){
        if(!isPoweredup){
            Act(ActionEnum.DOWN);
         }
    }

    void OnRIGHT(){
        if(!isPoweredup){
            Act(ActionEnum.RIGHT);
        }
    }

    void OnCoolDown(){
        isCoolDown = false; 
    }

    void PowerupAct(){
        if(GameStateManager.instance.CurrentState != GameStateManager.STATE.PLAYING) {
            return;
        }

        int collisionCount = SuccessCollider.OverlapCollider(contactFilter, collisions);
        for (int i = 0; i<collisionCount; i++) {
            GameObject collisionObj = collisions[i].gameObject;
            ActionEnum dir = ActionEnum.UP;
            switch (collisionObj.tag) {
                case "Obstacle":
                    ObstacleAScript obstacleScript = collisionObj.GetComponentInChildren<ObstacleAScript>();
                    // skip collisions unless the obstacle is in normal state
                    if (obstacleScript.GetCurrentState() != ObstacleAScript.STATE.Normal) {
                        continue;
                    }
                    OnObstacleResolution.Invoke();
                    obstacleScript.HandlePlayerResolvedThisObstacleSuccessfully();
                    dir = obstacleScript.getActionType().dir;
                    PlaySFX(dir);
                    ResetAllAnimationTriggers();
                    PlaySuccessAnimation(dir);
                    break;

				default: 
                    throw new System.Exception("should never happen");
               
            }
        }
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
        bool completeObstacle = false;

        int collisionCount = SuccessCollider.OverlapCollider(contactFilter, collisions);
        for (int i = 0; i<collisionCount; i++) {
            GameObject collisionObj = collisions[i].gameObject;
            string logText = $"player ({(isPoweredup ? "powered up" : "not powered up")}) success collision {i+1} of {collisionCount}: {collisionObj.name} (instanceid: {collisionObj.GetInstanceID()}, tag:{collisionObj.tag}). ";
           
            switch (collisionObj.tag) {
                case "Obstacle":
                    ObstacleAScript obstacleScript = collisionObj.GetComponentInChildren<ObstacleAScript>();
                    // skip collisions unless the obstacle is in normal state
                    if (obstacleScript.GetCurrentState() != ObstacleAScript.STATE.Normal) {
                        Log(logText + $"skipped because obstacle in ignored state {obstacleScript.GetCurrentState()}.");
                        continue;
                    }
                    // skip collisions if the player used the wrong key
                    if (obstacleScript.getActionType().dir != dir && !isPoweredup) {
                        Log(logText + $"skipped because action {obstacleScript.getActionType().dir} different from pressed {dir}.");
                        continue;
                    }
                    Log(logText + "obstacle collision resolved.");
                    OnObstacleResolution.Invoke();
                    timing.CheckTiming(collisions[i].bounds.min.x);
                    obstacleScript.HandlePlayerResolvedThisObstacleSuccessfully();
                    completeObstacle = true;
                    break;

				case "Powerup" :
                    PowerupAScript powerupScript = collisionObj.GetComponentInChildren<PowerupAScript>();
                    // skip collisions if the player used the wrong key
                    if (powerupScript.getActionType().dir != dir) {
                        Log(logText + $"skipped because action {powerupScript.getActionType().dir} different from pressed {dir}.");
                        continue;
                    }
                    Log(logText + "powerup collision resolved.");
                    PowerupManager.Instance.startPowerup();
                    OnPowerupResolution.Invoke();
                    powerupScript.changeState(PowerupAScript.STATE.PlayerResolvedSuccessfully);
                break;

				default: 
                    throw new System.Exception("should never happen");
               
            }
			
        }

        // invoke events (used to trigger fmod sounds)
        PlaySFX(dir);
        // start the animation for the corresponding action
        ResetAllAnimationTriggers();
        if(completeObstacle){
            PlaySuccessAnimation(dir);
        } else {
            PlayAnimation(dir);
        }
    }

    public void OnNewGame() {
        playerSpriteRenderer.enabled = true;
        animator.enabled = true;
        animator.speed = 1;
        animator.SetTrigger(TRIGGER_START_RUNNING);
    }

    public void OnFailure() {
        OnObstacleCollision.Invoke();
        animator.SetTrigger(TRIGGER_START_FAILURE);
    }

    private void ResetAllAnimationTriggers() {
        animator.ResetTrigger(TRIGGER_START_RUNNING);
        animator.ResetTrigger(TRIGGER_START_UP);
        animator.ResetTrigger(TRIGGER_START_RIGHT);
        animator.ResetTrigger(TRIGGER_START_DOWN);
        animator.ResetTrigger(TRIGGER_START_FAILURE);
        animator.ResetTrigger(TRIGGER_START_UP_SUCCESS);
    }

    public void PlaySFX(ActionEnum dir){
        switch (dir) {
            case ActionEnum.RIGHT: OnRightActionEvent.Invoke(); break;
            case ActionEnum.DOWN:  OnDownActionEvent.Invoke();  break;
            case ActionEnum.UP:    OnUpActionEvent.Invoke();    break;
        }
    }

    public void PlayAnimation(ActionEnum dir){
        switch (dir) {
            case ActionEnum.RIGHT: animator.SetTrigger(TRIGGER_START_RIGHT); break;
            case ActionEnum.DOWN:  animator.SetTrigger(TRIGGER_START_DOWN);  break;
            case ActionEnum.UP:    animator.SetTrigger(TRIGGER_START_UP);    break;
        }
    }

    public void PlaySuccessAnimation(ActionEnum dir){
        switch(dir){
            case ActionEnum.RIGHT: animator.SetTrigger(TRIGGER_START_RIGHT); break;
            case ActionEnum.DOWN:  animator.SetTrigger(TRIGGER_START_DOWN);  break;
            case ActionEnum.UP:    animator.SetTrigger(TRIGGER_START_UP_SUCCESS); break;
        }
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

