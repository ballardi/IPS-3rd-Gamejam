using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameStateManager : MonoBehaviour {

    public static GameStateManager instance;

    public enum STATE { 
        TITLE_SCREEN,
        PLAYING, 
        PAUSED,
        GAMEOVER };

    public STATE CurrentState { get; private set; }
    private float _CurrentSpeed;
    public int CurrentScore { get; private set; }
    private float _CurrentScoreFloat;

    public float InitialSpeed;
    public float MaxSpeed;
    public float SpeedAdditionPerSecond;
    public float MultiplierFromTimeToScore;

    void Awake() {
        
        Assert.IsNull(instance); instance = this; // singleton setup

        Assert.IsTrue(InitialSpeed > 0);
        Assert.IsTrue(SpeedAdditionPerSecond >= 0);
        Assert.IsTrue(MultiplierFromTimeToScore > 0);
    }

    // executed after all objects have executed their Awake()
    public void Start() {
        OnShowTitleScreen();
    }

    void Update() {
        if(CurrentState == STATE.PLAYING) {
            // figure out how much the speed and score should increase by
            float SpeedAddition = Time.deltaTime * SpeedAdditionPerSecond;
            // increase the score by that amount
            _CurrentScoreFloat += (SpeedAddition  * MultiplierFromTimeToScore);
            CurrentScore = Mathf.FloorToInt(_CurrentScoreFloat);
            // increase the speed by that amount, but only up to the max speed
            _CurrentSpeed = Mathf.Clamp(_CurrentSpeed + SpeedAddition, InitialSpeed, MaxSpeed);
        }
    }

    public float GetCurrentSpeed() {
        if(CurrentState == STATE.PLAYING)
            return _CurrentSpeed;
        else
            return 0;
    }

    public void OnShowTitleScreen() {
        TitleScreenScript.instance.Show(true); ;
        CurrentState = STATE.TITLE_SCREEN;
    }

    public void OnPlay() {

        switch (CurrentState) {
            case STATE.TITLE_SCREEN: // if titlescreen -> play
                _CurrentSpeed = InitialSpeed;
                _CurrentScoreFloat = 0;
                CurrentScore = 0;
                break;
            case STATE.PLAYING:  throw new System.Exception("should never happen");
            case STATE.PAUSED:   break;
            case STATE.GAMEOVER: throw new System.Exception("should never happen");
        }
        CurrentState = STATE.PLAYING;
    }

    public void OnPause() {
        switch (CurrentState) {
            case STATE.TITLE_SCREEN: throw new System.Exception("should never happen");
            case STATE.PLAYING:
                PauseMenuScript.instance.Show(true);
                break;
            case STATE.PAUSED:       throw new System.Exception("should never happen");
            case STATE.GAMEOVER:     throw new System.Exception("should never happen");
        }
        CurrentState = STATE.PAUSED;
    }

    public void OnGameOver() {
        switch (CurrentState)
        {
            case STATE.TITLE_SCREEN: throw new System.Exception("should never happen");
            case STATE.PLAYING:      break;
            case STATE.PAUSED:       throw new System.Exception("should never happen");
            case STATE.GAMEOVER:     throw new System.Exception("should never happen");
        }
        CurrentState = STATE.GAMEOVER;
    }

}

