using ObstacleManagement;
using PowerupManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public enum STATE
    {
        TITLE_SCREEN,
        PLAYING,
        PAUSED,
        GAMEOVER
    };

    public STATE CurrentState { get; private set; }
    private float _CurrentSpeed;
    public int CurrentScore { get; private set; }
    private float _CurrentScoreFloat;

    public int HighScore { get; private set; }

    public float InitialSpeed;
    public float MaxSpeed;
    public float SpeedAdditionPerSecond;
    public float MultiplierFromTimeToScore;
    private float timeSinceLastSpeedIncrease;

    // fps we don't want unity to exceed
    public int targetFrameRate;

    // how much should one unit of speed convert to in distance of default units on the x axis
    public float SpeedToDistanceConversionMultiplier;

    public UnityEvent OnTitleScreenStartEvent;
    public UnityEvent OnNewGameEvent;
    public UnityEvent OnPauseEvent;
    public UnityEvent OnUnpauseEvent;
    public UnityEvent OnGameoverEvent;

    public bool HasPlayerEverSeenTutorial_Up { get; private set; }
    public bool HasPlayerEverSeenTutorial_Right { get; private set; }
    public bool HasPlayerEverSeenTutorial_Down { get; private set; }

    void Awake()
    {
        Assert.IsNull(instance);
        instance = this; // singleton setup

        Assert.IsTrue(targetFrameRate > 0);
        Application.targetFrameRate = targetFrameRate;

        Assert.IsTrue(SpeedToDistanceConversionMultiplier > 0);
        Assert.IsTrue(InitialSpeed > 0);
        Assert.IsTrue(SpeedAdditionPerSecond >= 0);
        Assert.IsTrue(MultiplierFromTimeToScore > 0);

        HasPlayerEverSeenTutorial_Up = false;
        HasPlayerEverSeenTutorial_Right = false;
        HasPlayerEverSeenTutorial_Down = false;
    }

    // executed after all objects have executed their Awake()
    public void Start()
    {
        OnShowTitleScreen();
    }

    void Update()
    {
        if (CurrentState == STATE.PLAYING)
        {
            // figure out how much the speed and score should increase by
            float speedIncreaseAmount = 0;
            timeSinceLastSpeedIncrease += Time.deltaTime;
            while (timeSinceLastSpeedIncrease >= 1.0f){
                speedIncreaseAmount += SpeedAdditionPerSecond;
                timeSinceLastSpeedIncrease -= 1.0f;
            }


            // increase the speed by that amount, but only up to the max speed
            _CurrentSpeed = Mathf.Clamp(_CurrentSpeed + speedIncreaseAmount, InitialSpeed, MaxSpeed);
            
            // increase the score by the distance traveled (calculated by speed before any update)
            _CurrentScoreFloat += _CurrentSpeed * Time.deltaTime * MultiplierFromTimeToScore;
            CurrentScore = Mathf.FloorToInt(_CurrentScoreFloat);
        }
    }

    public float GetDistanceToTravelThisFrame()
    {
        if (CurrentState == STATE.PLAYING)
            return _CurrentSpeed * Time.deltaTime * SpeedToDistanceConversionMultiplier;
        else
            return 0;
    }

    public void OnShowTitleScreen()
    {
        if (CurrentScore > HighScore)
            HighScore = CurrentScore;

        PlayerScript.instance.OnStartTitleScreen();
        TitleScreenScript.instance.Show(true);
        ScoreScript.instance.Show(false);
        OnTitleScreenStartEvent.Invoke();
        CurrentState = STATE.TITLE_SCREEN;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ObjectPool")) {
            obj.GetComponent<ObjectPool>().OnStartNewGame();
        }
    }

    public void OnPlay()
    {
        switch (CurrentState)
        {
            case STATE.GAMEOVER:
            case STATE.TITLE_SCREEN:
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ObjectPool")) {
                    obj.GetComponent<ObjectPool>().OnStartNewGame();
                }
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Powerup")) {
                    obj.GetComponent<PowerupAScript>().DespawnPowerup();
                }
                _CurrentSpeed = InitialSpeed;
                _CurrentScoreFloat = 0;
                CurrentScore = 0;
                timeSinceLastSpeedIncrease = 0;
                PauseButtonScript.instance.Show(true);
                ScoreScript.instance.Show(true);
                PlayerScript.instance.OnNewGame();
                ObstacleManager.Instance.OnGameStart();
                PowerupManager.Instance.OnGameStart();
                OnNewGameEvent.Invoke();
                break;
            case STATE.PLAYING:
                throw new System.Exception("should never happen");
            case STATE.PAUSED:
                PauseButtonScript.instance.Show(true);
                PlayerScript.instance.OnUnpause();
                OnUnpauseEvent.Invoke();
                break;
        }
        CurrentState = STATE.PLAYING;
    }

    public void OnPause()
    {
        switch (CurrentState)
        {
            case STATE.TITLE_SCREEN:
                throw new System.Exception("should never happen");
            case STATE.PLAYING:
                OnPauseEvent.Invoke();
                PauseButtonScript.instance.Show(false);
                PauseMenuScript.instance.Show(true);
                PlayerScript.instance.OnPause();
                break;
            case STATE.PAUSED:
                throw new System.Exception("should never happen");
            case STATE.GAMEOVER:
                throw new System.Exception("should never happen");
        }
        CurrentState = STATE.PAUSED;
    }

    public void OnGameOver()
    {
        switch (CurrentState)
        {
            case STATE.TITLE_SCREEN:
                throw new System.Exception("should never happen");
            case STATE.PLAYING:
                OnGameoverEvent.Invoke();
                PauseButtonScript.instance.Show(false);
                GameOverScreenScript.instance.Show(true);

                if(CurrentScore > HighScore)
                    HighScore = CurrentScore;

                break;
            case STATE.PAUSED:
                throw new System.Exception("should never happen");
            case STATE.GAMEOVER:
                throw new System.Exception("should never happen");
        }
        CurrentState = STATE.GAMEOVER;
    }
    
    public bool HasPlayerSeenAllActionTutorials() {
        return HasPlayerEverSeenTutorial_Down && HasPlayerEverSeenTutorial_Right && HasPlayerEverSeenTutorial_Up;
    }

    public bool HasPlayerSeenActionTutorial(ActionEnum action) {
        switch (action) {
            case ActionEnum.RIGHT: return HasPlayerEverSeenTutorial_Right;
            case ActionEnum.DOWN:  return HasPlayerEverSeenTutorial_Down;
            case ActionEnum.UP:    return HasPlayerEverSeenTutorial_Up;
            default: throw new System.Exception("should never happen");
        }
    }

    public void RegisterATutorialWasShown(ActionEnum action) {
        switch (action) {
            case ActionEnum.RIGHT: HasPlayerEverSeenTutorial_Right = true; break;
            case ActionEnum.DOWN:  HasPlayerEverSeenTutorial_Down  = true; break;
            case ActionEnum.UP:    HasPlayerEverSeenTutorial_Up    = true; break;
            default: throw new System.Exception("should never happen");
        }
    }

}
