using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer2 {

    private float _totalGameTimeBetweenAlerts;
    private float _remainingGameTimeUntillNextAlert;

    public Timer2(float gameTimeBetweenAlerts) {
        _totalGameTimeBetweenAlerts = gameTimeBetweenAlerts;
        _remainingGameTimeUntillNextAlert = _totalGameTimeBetweenAlerts;
    }

    /// <summary>
    /// call from monobehaviors' Update methods each frame. 
    /// if it returns true, then the timer is alerting you.
    /// </summary>
    /// <param name="deltaTimeSinceLastFrame">intended for you to pass the value of Time.delta</param>
    /// <returns></returns>
    public bool UpdateTimerProgress(float deltaTimeSinceLastFrame) {
        _remainingGameTimeUntillNextAlert -= deltaTimeSinceLastFrame;
        if (_remainingGameTimeUntillNextAlert < 0) {
            // when alert should be triggered, don't just always set remaining time to be equal to total time, because
            // this method might be getting called some time after the alert should have happened. so subtract that difference
            // from the remaining time before the following alert.
            _remainingGameTimeUntillNextAlert = _totalGameTimeBetweenAlerts - Mathf.Abs(_remainingGameTimeUntillNextAlert);
            return true;
        } else {
            return false;
        }
    }

    public void UpdateGameTimeBetweenAlerts(float gameTimeBetweenAlerts) => _totalGameTimeBetweenAlerts = gameTimeBetweenAlerts;

    public void ResetRemainingTimeToFullAmount() => _remainingGameTimeUntillNextAlert = _totalGameTimeBetweenAlerts;

    public float GetTotalTimeBetweenAlerts() => _totalGameTimeBetweenAlerts;

}
