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
    [Header("Perfect Range")]
    [SerializeField][Range(0.1f,1.0f)] private float perfectRatio = 0.2f;
    private float perfect_left;
    private float perfect_right;
    private Timer2 visibleTimer;
    [SerializeField] private float timerLength_seconds;
    private bool isOn = false;

    private void Awake()
    {
        Assert.IsNotNull(successCollider);
        Assert.IsNotNull(perfectDisplay);
        Assert.IsNotNull(lateDisplay);
        Assert.IsNotNull(earlyDisplay);
        if (timerLength_seconds <= 0.0f) timerLength_seconds = 0.1f;
        if (perfectRatio > 1.0f) perfectRatio = 1.0f;
        if (perfectRatio <= 0.0f) perfectRatio = .01f;

        CloseDisplays();

        visibleTimer = new Timer2(timerLength_seconds);
        CalculateBoundaries();
    }

    private void Update()
    {
        if(GameStateManager.instance.CurrentState != GameStateManager.STATE.PLAYING){
            CloseDisplays();
        }
        if(GameStateManager.instance.CurrentState == GameStateManager.STATE.PLAYING && isOn){
           bool isDone = visibleTimer.UpdateTimerProgress(Time.deltaTime);
           if(isDone){
            isOn = false;
            CloseDisplays();
           }
        }
    }

    private void CalculateBoundaries(){
        Bounds collider = successCollider.bounds;
        float far_left = collider.min.x;
        float far_right = collider.max.x; 
        float middle = (far_left+far_right)/(2.0f);
        float length = far_right - far_left; 
        perfect_left = middle - (length * (perfectRatio/2.0f));
        perfect_right = middle + (length * (perfectRatio/2.0f));
    }

    public void CheckTiming(float leftEdge){
        if(leftEdge < perfect_left){
            lateDisplay.SetActive(true);
        }
        if(leftEdge > perfect_right){
            earlyDisplay.SetActive(true);
        }
        if(leftEdge <= perfect_right && leftEdge >= perfect_left){
            perfectDisplay.SetActive(true);
        }
        isOn = true;
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
    //     float perfect_left = middle - (length * (perfectRatio/2.0f));
    //     float perfect_right = middle + (length * (perfectRatio/2.0f));

    //     if (!successCollider) return;
    //     float top = successCollider.bounds.max.y;
    //     float bottom = successCollider.bounds.min.y;
    
    //     Gizmos.color = Color.black;
    //     Gizmos.DrawSphere(new Vector2(perfect_left, top), 10f);
    //     Gizmos.DrawLine(new Vector2(perfect_left, top), new Vector2(perfect_left, bottom));
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(new Vector2(perfect_right, top), 5f);
    //     Gizmos.DrawLine(new (perfect_right, top), new Vector2(perfect_right, bottom));
    // }
}
