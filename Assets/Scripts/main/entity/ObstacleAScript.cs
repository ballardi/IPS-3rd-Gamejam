using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObstacleAScript : MonoBehaviour {

    public float startSpeed = 1;
    private float currentSpeed;
    private STATE currentState;

    public enum STATE { Normal, PlayerResolvedSuccessfully, PlayerFailed };

    [SerializeField]
    private ActionType ActionType;

    void Awake() {
        InitializeNewGame();
        Assert.IsNotNull(ActionType);
    }

    public void InitializeNewGame() {
        currentSpeed = startSpeed;
        currentState = STATE.Normal;
    }

    void Update() {
        if(currentState == STATE.Normal || currentState == STATE.PlayerResolvedSuccessfully) {
            // TODO: increase speed based on combination of deltaTime and some global game speed variable?
            Vector3 currentPos = transform.localPosition;
            transform.localPosition = new Vector3(currentPos.x - currentSpeed, currentPos.y, currentPos.z);
        }
    }

    // TODO: depending on the new state, do things like...
    // TODO: switch animation state (using animation trigger)
    // TODO: enable/disable collider
    // TODO: use Invoke(...) to disable the visual and movement after some time?
    public void changeState(STATE newState) {

        switch (newState) {
            case STATE.PlayerResolvedSuccessfully:
                gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                break;
            // TODO: handle other state changes
        }

        currentState = newState;
    }

    public ActionType getActionType() {
        //returning the scriptable obj instead of just the Enum in case future data is added.
        return ActionType;
    }
}
