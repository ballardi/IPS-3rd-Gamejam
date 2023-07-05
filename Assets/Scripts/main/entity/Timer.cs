using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// The Timer class is used to provide child classes with the ability to
/// start a timer and be notified when it ends.
/// </summary>
public abstract class Timer : MonoBehaviour
{
    /// <summary>
    /// The parent class of the timer is a MonoBehaviour, which manages the timer.
    /// A timer is not a standalone module, but rather needs to communicate with its manager
    /// to get started and alert it once the timer has ended.
    /// </summary>
    protected readonly MonoBehaviour _parent;

    /// <summary>
    /// The amount of seconds until the timer ends
    /// </summary>
    protected float _duration;

    /// <summary>
    /// The amount of seconds that have passed since the timer started
    /// </summary>
    protected float _time_seconds;

    /// <summary>
    /// Used to track if this timer is running at the moment.
    /// </summary>
    private bool isOn;

    /// <summary>
    /// Creates the timer using a manager MonoBehaviour as its parent.
    /// Throws an exception if the parent is null.
    /// </summary>
    /// <param name="parent">The non-null MonoBehaviour script managing this timer</param>
    protected Timer(MonoBehaviour parent)
    {
        if (parent == null)
            throw new ArgumentNullException("parent");

        _parent = parent;
    }

    /// <summary>
    /// Sets up the timer.
    /// Uses the current spawn rate as the duration of the timer in seconds.
    /// If the timer is running, an Exception is thrown.
    /// </summary>
    public void Awake()
    {
        _time_seconds = 0f;
        isOn = false;
    }

    public void Update()
    {
        if(isOn){
            Tick();
            Debug.Log($"{_parent} is at {_time_seconds}");
        }
    }

    /// <summary>
    /// Stops the timer by stopping the coroutine in the parent.
    /// </summary>
    public void Stop()
    {
        isOn = false; 
        _time_seconds = 0f;
    }

    /// <summary>
    /// Pauses the timer. 
    /// </summary>
    public void Pause()
    {
        isOn = false;
    }

    public void StartTimer()
    {
        isOn = true;
    }

    /// <summary>
    /// Waits the specified duration and then alerts the extending class
    /// that the timer has ended.
    /// </summary>
    protected void Tick()
    {
        _time_seconds++;
        if(_time_seconds >= _duration){
            Alert();
        }
        
    }

    /// <summary>
    /// Alert is called whenever the timer ends.
    /// Child classes can use this to program the desired behaviour.
    /// </summary>
    protected abstract void Alert();
}
