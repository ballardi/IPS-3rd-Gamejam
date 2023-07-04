
using UnityEngine;
using UnityEngine.Assertions;

public class ObstacleAScript : LoggableMonoBehaviour, IPoolable {

    public float startSpeed = 1;
    private float currentSpeed;
    private STATE currentState;

    public enum STATE { Normal, PlayerResolvedSuccessfully, PlayerFailed, Hidden };

    [SerializeField] private ActionType ActionType;

    private Color startingColor;

    void Awake() {
        Assert.IsNotNull(ActionType);
        startingColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
    }

    void Update() {
        if (currentState == STATE.Normal || currentState == STATE.PlayerResolvedSuccessfully) {
            currentSpeed = GameStateManager.instance.GetCurrentSpeed();
            Vector3 currentPos = transform.localPosition;
            transform.localPosition = new Vector3(currentPos.x - currentSpeed, currentPos.y, currentPos.z);
        }
    }

    // TODO: depending on the new state, do things like...
    // TODO: switch animation state (using animation trigger)
    private void changeState(STATE newState) {

        switch (newState) {
            case STATE.Normal: // basically what to do when initialized
                currentSpeed = GameStateManager.instance.GetCurrentSpeed();
                gameObject.GetComponentInChildren<SpriteRenderer>().color = startingColor;
                gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case STATE.PlayerResolvedSuccessfully: 
                gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case STATE.PlayerFailed:
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case STATE.Hidden:
                currentSpeed = GameStateManager.instance.GetCurrentSpeed();
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
