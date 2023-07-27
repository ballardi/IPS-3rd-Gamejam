using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TimingScript : MonoBehaviour
{
    [Header("Success Collider")]
    [SerializeField] private BoxCollider2D successCollider;
    [Header("Timing Displays")]
    [SerializeField] private GameObject perfectDisplay;
    [SerializeField] private GameObject lateDisplay;
    [SerializeField] private GameObject earlyDisplay;
    [Header("Timing Section Ratio")]
    [SerializeField][Range(0.01f,0.5f)] private float lateRatio = 0.2f;
    [SerializeField][Range(0.01f,0.5f)] private float earlyRatio = 0.2f;
    private float perfect_left;
    private float perfect_right;

    private Timer2 perfectTimer;
    private Timer2 lateTimer;
    private Timer2 earlyTimer;
    [SerializeField] private float timerLength_seconds;

    private bool isPerfectOn = false;
    private bool isLateOn = false;
    private bool isEarlyOn = false;

    private void Awake()
    {
        Assert.IsNotNull(successCollider);
        Assert.IsNotNull(perfectDisplay);
        Assert.IsNotNull(lateDisplay);
        Assert.IsNotNull(earlyDisplay);
        if (timerLength_seconds <= 0.0f) timerLength_seconds = 0.1f;
        if (earlyRatio > 0.5f) earlyRatio = 0.5f;
        if (lateRatio > 0.5f) lateRatio = 0.5f;
        if (earlyRatio <= 0.0f) earlyRatio = 0.1f;
        if (lateRatio <= 0.0f)  lateRatio = 0.1f;

        CloseDisplays();

        perfectTimer = new Timer2(timerLength_seconds);
        lateTimer = new Timer2(timerLength_seconds);
        earlyTimer = new Timer2(timerLength_seconds);
        CalculateBoundaries();
    }

    private void Update()
    {
        if(GameStateManager.instance.CurrentState != GameStateManager.STATE.PLAYING){
            CloseDisplays();
        }
        if(GameStateManager.instance.CurrentState == GameStateManager.STATE.PLAYING){
            if(isEarlyOn){
                bool isDone = earlyTimer.UpdateTimerProgress(Time.deltaTime);
                if(isDone){
                    isEarlyOn = false;
                    earlyDisplay.SetActive(false);
                }
            }
            if(isLateOn){
                bool isDone = lateTimer.UpdateTimerProgress(Time.deltaTime);
                if(isDone){
                    isLateOn = false;
                    lateDisplay.SetActive(false);
                }
            }
            if(isPerfectOn){
                bool isDone = perfectTimer.UpdateTimerProgress(Time.deltaTime);
                if(isDone){
                    isPerfectOn = false;
                    perfectDisplay.SetActive(false);
                }
            }
          
        }
    }

    private void CalculateBoundaries(){
        Bounds collider = successCollider.bounds;
        float far_left = collider.min.x;
        float far_right = collider.max.x; 
        float middle = (far_left+far_right)/(2.0f);
        float length = far_right - far_left; 
        perfect_left = far_left + (length * lateRatio);
        perfect_right = far_right - (length * earlyRatio);
    }

     /// <summary>
    /// Checks the timing of the player clearing an obstacle.
    /// </summary>
    /// <param name="leftEdge">The X coordinate of the left most collder edge of the obstacle.</param>
    /// <returns></returns>
    public void CheckTiming(float leftEdge){
        if(leftEdge < perfect_left){
            lateDisplay.SetActive(true);
            isLateOn = true;
            lateTimer.ResetRemainingTimeToFullAmount();
        }
        if(leftEdge > perfect_right){
            earlyDisplay.SetActive(true);
            isEarlyOn = true;
            earlyTimer.ResetRemainingTimeToFullAmount();
        }
        if(leftEdge <= perfect_right && leftEdge >= perfect_left){
            perfectDisplay.SetActive(true);
            isPerfectOn = true;
            perfectTimer.ResetRemainingTimeToFullAmount();
        }
    }

    private void CloseDisplays(){
        perfectDisplay.SetActive(false);
        lateDisplay.SetActive(false);
        earlyDisplay.SetActive(false);
    }

    // REMOVE COMMENTS FOR DEBUGGING

    // void OnDrawGizmos()
    // {
    //     Bounds collider = successCollider.bounds;
    //     float far_left = collider.min.x;
    //     float far_right = collider.max.x; 
    //     float middle = (far_left+far_right)/(2.0f);
    //     float length = far_right - far_left; 
    //     float perfect_left = far_left + (length * lateRatio);
    //     float perfect_right = far_right - (length * earlyRatio);

    //     if (!successCollider) return;
    //     float top = successCollider.bounds.max.y;
    //     float bottom = successCollider.bounds.min.y;
    
    //     Gizmos.color = Color.black;
    //     Gizmos.DrawSphere(new Vector2(perfect_left, top), 5f);
    //     Gizmos.DrawLine(new Vector2(perfect_left, top), new Vector2(perfect_left, bottom));
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(new Vector2(perfect_right, top), 5f);
    //     Gizmos.DrawLine(new (perfect_right, top), new Vector2(perfect_right, bottom));
    // }
}
