using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using PowerupManagement;

public class PowerupAScript : MonoBehaviour {

    public float startSpeed = 1;
    private float currentSpeed;
    private STATE currentState;

    public enum STATE { Normal, PlayerResolvedSuccessfully, PlayerFailed };

    [SerializeField]
    private ActionType ActionType;

    [SerializeField]
    private float powerupLength_seconds = 3.0f;

    private bool powerup_used = false;

    void Awake() {
        InitializeNewGame();
        Assert.IsNotNull(ActionType);
    }

    public void InitializeNewGame() {
        currentSpeed = GameStateManager.instance.GetCurrentSpeed();
        currentState = STATE.Normal;
    }

    void Update() {
        currentSpeed = GameStateManager.instance.GetCurrentSpeed();
        //First one should handle position movement
        if (currentState == STATE.Normal || currentState == STATE.PlayerResolvedSuccessfully) {
            Vector3 currentPos = transform.localPosition;
            transform.localPosition = new Vector3(currentPos.x - currentSpeed, currentPos.y, currentPos.z);
        }
        //This should handle the timer notification/dissappearance
        if (currentState == STATE.PlayerResolvedSuccessfully) {
            if(!powerup_used){
                powerup_used = true; // MUST be before the powerupmanager function call.
                PowerupManager.Instance.StartPowerupTimer(powerupLength_seconds);
            }
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
