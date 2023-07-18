using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PowerupIndicator : MonoBehaviour
{
    //ask if there should be any reason this is a singleton...
    public static PowerupIndicator Instance {private set; get;}
    [SerializeField] private Timer2 _timer;
    [SerializeField] private GameObject powerUpIndicator; 
    [SerializeField] private Animator anim;
    private bool willFlash = false; 

    private void Awake(){
        Assert.IsNull(Instance, "The PowerupIndicator should be null");
        Instance = this;

        Assert.IsNotNull(powerUpIndicator);
        powerUpIndicator.SetActive(false);
        Assert.IsNotNull(anim);
        // declared here so that there's not a new instantation everytime
        // a power up is collected;
        _timer = new Timer2(3000f);
    }

    private void Update()
    {
        if (GameStateManager.instance.CurrentState != GameStateManager.STATE.PLAYING){
            return;
        }

        if (willFlash){
            bool timerAlerted = _timer.UpdateTimerProgress(Time.deltaTime);
            if(timerAlerted){
                willFlash = false; 
                StartFlashing();
            }
        }
    }
    

    public void StartPowerupIndicator(float FLASHING_LENGTH, float POWERUP_LENGTH){
        powerUpIndicator.SetActive(true);
        willFlash = true;
        _timer.UpdateGameTimeBetweenAlerts(POWERUP_LENGTH - FLASHING_LENGTH);
        _timer.ResetRemainingTimeToFullAmount();
    }

    public void StartFlashing(){
        anim.SetTrigger("Flashing");
    }

    public void EndPowerupIndicator(){
        willFlash = false;
        anim.ResetTrigger("Flashing");
        powerUpIndicator.SetActive(false);
        _timer.UpdateGameTimeBetweenAlerts(3000f);
        _timer.ResetRemainingTimeToFullAmount();
    }

}
