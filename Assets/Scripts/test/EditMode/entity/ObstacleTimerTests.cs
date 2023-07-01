using System;
using NUnit.Framework;
using ObstacleManagement;
using UnityEngine;
using System.Reflection;

public class ObstacleTimerTests
{
    private ObstacleManager _manager;
    private ObstacleTimer _timer;

    [SetUp]
    public void BeforeAll()
    {
        var mockObject = new GameObject().AddComponent<ObstacleManager>();
        _manager = mockObject.GetComponent<ObstacleManager>();
        _manager.DebugMode = true;
        _timer = new(_manager);
    }

    /// <summary>
    /// Tests if, when trying to create a new ObstacleTimer instance,
    /// but supplying null for the manager parent, an ArgumentNullException is thrown.
    /// </summary>
    [Test]
    public void Instantiating_ObstacleTimer_Without_Parent_Throws_Exception()
    {
        Assert.Throws<ArgumentNullException>(() => _timer = new(null));
    }

    /// <summary>
    /// Tests if the duration of the timer is set to the initial spawn rate provided
    /// by the ObstacleTimer test.
    /// </summary>
    [Test]
    public void Timer_Duration_Is_Initialised_To_Initial_Spawn_Rate()
    {
        FieldInfo durationField = typeof(Timer).GetField(
            "_duration",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        var duration = (float)durationField.GetValue(_timer);
        Assert.AreEqual(ObstacleTimer.INITIAL_SPAWN_RATE, duration);
    }

    // TODO: DurationDecrement tests
}
