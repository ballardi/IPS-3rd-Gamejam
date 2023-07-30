using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using FMODUnity;


public class PauseButtonScript : MonoBehaviour {
    

    public GameObject objToShow;

    public static PauseButtonScript instance;

  

    public StudioEventEmitter emitter;

    private void Awake() {
        Assert.IsNull(instance); instance = this; // singleton logic

        Assert.IsNotNull(objToShow);

        Assert.IsNotNull(emitter);

     


        Show(false);
    }

    public void Show(bool show) {
        objToShow.SetActive(show);
    }


    public void OnPauseButtonClick() {
        GameStateManager.instance.OnPause();

        
            var instance = emitter.EventInstance;
            instance.setPaused(true);
        

    }

}
