
using UnityEngine;
using UnityEngine.Assertions;

public class ObstacleAScript : LoggableMonoBehaviour, IPoolable {

    public float startSpeed = 1;
    private float distanceToTravel;
    private STATE currentState;

    public enum STATE { Normal, PlayerResolvedSuccessfully, PlayerFailed, Hidden };

    [SerializeField] private ActionType ActionType;

    private SpriteRenderer spriteRenderer;
    private Timer2 visibilityTimer;
    private const float SPRITE_OFF_SECONDS = .65f;
    private bool spriteOff = false; 

    [SerializeField] private Transform SuccessPositionUP, SuccessPositionDOWN, SuccessPositionLeft;

    void Awake() {
        Assert.IsNotNull(ActionType);
        Assert.IsNotNull(SuccessPositionUP);
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        visibilityTimer = new Timer2(SPRITE_OFF_SECONDS);
    }

    void Update() {
        if ((currentState == STATE.Normal || currentState == STATE.PlayerResolvedSuccessfully) && !spriteOff) {
            distanceToTravel = GameStateManager.instance.GetDistanceToTravelThisFrame();
            Vector3 currentPos = transform.localPosition;
            transform.localPosition = new Vector3(currentPos.x - distanceToTravel, currentPos.y, currentPos.z);
        }
        if(spriteOff){
            bool isCompleted = visibilityTimer.UpdateTimerProgress(Time.deltaTime);
            if(isCompleted){
                spriteOff = false; 
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = ActionType.success_image;
            }
        }
    }

    // TODO: depending on the new state, do things like...
    // TODO: switch animation state (using animation trigger)
    private void changeState(STATE newState) {

        switch (newState) {
            case STATE.Normal: // basically what to do when initialized
                distanceToTravel = GameStateManager.instance.GetDistanceToTravelThisFrame();
                gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case STATE.PlayerResolvedSuccessfully: 
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                if(ActionType.dir == ActionEnum.UP){
                    spriteRenderer.enabled = false;
                    spriteOff = true;
                    transform.position = SuccessPositionUP.position;
                }
                break;
            case STATE.PlayerFailed:
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case STATE.Hidden:
                distanceToTravel = GameStateManager.instance.GetDistanceToTravelThisFrame();
                if(ActionType.dir == ActionEnum.UP){
                    spriteRenderer.sprite = ActionType.default_image;
                }
                gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                break;
            default:
                throw new System.Exception("should never happen");
        }

        currentState = newState;
    }

    public void HandlePlayerResolvedThisObstacleSuccessfully() {
        changeState(STATE.PlayerResolvedSuccessfully);
    }

    public void HandlePlayerFailedBecauseOfThisObstacle() {
        changeState(STATE.PlayerFailed);
    }

    public void HandleCollisionWithDespawnerZone() {
        changeState(STATE.Hidden);
        GetComponentInParent<ObjectPool>().ChangeObjBackToAvailable(gameObject);
    }

    public STATE GetCurrentState() {
        return currentState;
    }

    public ActionType getActionType() {
        //returning the scriptable obj instead of just the Enum in case future data is added.
        return ActionType;
    }
    
    void IPoolable.InitializeOnUse() {
        changeState(STATE.Normal);
    }
    
    void IPoolable.DeInitializeOnPooling() {
        changeState(STATE.Hidden);
    }
    
}
