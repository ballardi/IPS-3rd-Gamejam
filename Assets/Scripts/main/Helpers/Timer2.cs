using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer2 {

    private float _totalGameTimeBetweenAlerts;
    private float _remainingGameTimeUntillNextAlert;

    private bool _isGameTimePaused;

    public Timer2(float gameTimeBetweenAlerts) {
        _totalGameTimeBetweenAlerts = gameTimeBetweenAlerts;
        _remainingGameTimeUntillNextAlert = _totalGameTimeBetweenAlerts;
        _isGameTimePaused = false;
    }

    /// <summary>
    /// call from monobehaviors' Update methods each frame. 
    /// if it returns true, then the timer is alerting you.
    /// </summary>
    /// <param name="deltaTimeSinceLastFrame">intended for you to pass the value of Time.delta</param>
    /// <returns></returns>
    public bool UpdateTimerProgress(float deltaTimeSinceLastFrame) {
        if (_isGameTimePaused) {
            return false;
        }

        _remainingGameTimeUntillNextAlert -= deltaTimeSinceLastFrame;
        if (_remainingGameTimeUntillNextAlert < 0) {
            _remainingGameTimeUntillNextAlert = _totalGameTimeBetweenAlerts - Mathf.Abs(_remainingGameTimeUntillNextAlert);
            return true;
        } else {
            return false;
        }
    }

    public void UpdateGameTimeBetweenAlerts(float gameTimeBetweenAlerts) => _totalGameTimeBetweenAlerts = gameTimeBetweenAlerts;

    public void SetGameTimerIsPaused(bool isPaused) => _isGameTimePaused = isPaused;

    public void ResetRemainingTimeToFullAmount() => _remainingGameTimeUntillNextAlert = _totalGameTimeBetweenAlerts;

    public float GetTotalTimeBetweenAlerts() => _totalGameTimeBetweenAlerts;

}
