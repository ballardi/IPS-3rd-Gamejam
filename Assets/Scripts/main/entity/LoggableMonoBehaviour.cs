using System;
using UnityEngine;

/// <summary>
/// Equips MonoBehaviours with the ability to enable and disable a DebugMode,
/// which logs messages to Unity's console.
/// </summary>
public abstract class LoggableMonoBehaviour : MonoBehaviour
{
    /// <summary>
    /// Determines if the log messages should be printed to the console
    /// </summary>
    public bool DebugMode;

    /// <summary>
    /// Logs the message to Unity's console if the DebugMode is set to `true`
    /// </summary>
    /// <param name="message">The non-null message consisting of at least one non-whitespace token</param>
    protected void Log(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Log Message must not be empty");

        if (DebugMode)
            Debug.Log(message);
    }
}
