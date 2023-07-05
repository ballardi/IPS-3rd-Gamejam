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

    public float InitialSpeed;
    public float MaxSpeed;
    public float SpeedAdditionPerSecond;
    public float MultiplierFromTimeToScore;

    public UnityEvent OnTitleScreenStartEvent;
    public UnityEvent OnNewGameEvent;
    public UnityEvent OnPauseEvent;
    public UnityEvent OnUnpauseEvent;
    public UnityEvent OnGameoverEvent;

    void Awake()
    {
        Assert.IsNull(instance);
        instance = this; // singleton setup

        Assert.IsTrue(InitialSpeed > 0);
        Assert.IsTrue(SpeedAdditionPerSecond >= 0);
        Assert.IsTrue(MultiplierFromTimeToScore > 0);
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
            float SpeedAddition = Time.deltaTime * SpeedAdditionPerSecond;
            // increase the score by that amount
            _CurrentScoreFloat += SpeedAddition * MultiplierFromTimeToScore;
            CurrentScore = Mathf.FloorToInt(_CurrentScoreFloat);
            // increase the speed by that amount, but only up to the max speed
            _CurrentSpeed = Mathf.Clamp(_CurrentSpeed + SpeedAddition, InitialSpeed, MaxSpeed);
        }
    }

    public float GetCurrentSpeed()
    {
        if (CurrentState == STATE.PLAYING)
            return _CurrentSpeed;
        else
            return 0;
    }

    public void OnShowTitleScreen()
    {
        PlayerScript.instance.OnStartTitleScreen();
        TitleScreenScript.instance.Show(true);
        OnTitleScreenStartEvent.Invoke();
        CurrentState = STATE.TITLE_SCREEN;
        // PowerupManager.Instance.OnGameEnd();
        ObstacleManager.Instance.OnGameEnd();
    }

    public void OnRestart()
    {
        // PowerupManager.Instance.OnGameEnd();
        ObstacleManager.Instance.OnGameEnd();
        // TODO: Actually implement
    }

    public void OnPlay()
    {
        switch (CurrentState)
        {
            case STATE.GAMEOVER:
            case STATE.TITLE_SCREEN:
                _CurrentSpeed = InitialSpeed;
                _CurrentScoreFloat = 0;
                CurrentScore = 0;
                PauseButtonScript.instance.Show(true);
                PlayerScript.instance.OnNewGame();
                ObstacleManager.Instance.OnGameStart();
                // PowerupManager.Instance.OnGameStart();
                OnNewGameEvent.Invoke();
                break;
            case STATE.PLAYING:
                throw new System.Exception("should never happen");
            case STATE.PAUSED:
                PauseButtonScript.instance.Show(true);
                PlayerScript.instance.OnUnpause();
                OnUnpauseEvent.Invoke();
                ObstacleManager.Instance.UnPause();
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
                ObstacleManager.Instance.OnPause();
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
                ObstacleManager.Instance.OnGameEnd();
                // PowerupManager.Instance.OnGameEnd();
                break;
            case STATE.PAUSED:
                throw new System.Exception("should never happen");
            case STATE.GAMEOVER:
                throw new System.Exception("should never happen");
        }
        CurrentState = STATE.GAMEOVER;
    }
}
