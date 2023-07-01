using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// The Timer class is used to provide child classes with the ability to
/// start a timer and be notified when it ends.
/// </summary>
public abstract class Timer
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
    /// Used to track if this timer is running at the moment.
    /// If it is, the system should not try to start it.
    /// </summary>
    private Coroutine _clock;

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
    /// Starts the timer if it is not already running.
    /// Uses the current spawn rate as the duration of the timer in seconds.
    /// If the timer is running, an Exception is thrown.
    /// </summary>
    public void Start()
    {
        Assert.IsNull(_clock, "Should not start a timer if it is already running!");
        _clock = _parent.StartCoroutine(Tick());
    }

    /// <summary>
    /// Waits the specified duration and then alerts the extending class
    /// that the timer has ended.
    /// </summary>
    /// <returns>An IEnumerator in order to use the WaitForSeconds() method</returns>
    protected IEnumerator Tick()
    {
        yield return new WaitForSeconds(_duration);
        _clock = null;
        Alert();
    }

    /// <summary>
    /// Alert is called whenever the timer ends.
    /// Child classes can use this to program the desired behaviour.
    /// </summary>
    protected abstract void Alert();
}
