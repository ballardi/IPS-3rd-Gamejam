using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using FMODUnity;

public class PauseMenuScript : MonoBehaviour {

    public GameObject objToShow;

    public static PauseMenuScript instance;

    public StudioEventEmitter emitter;

    private void Awake() {
        Assert.IsNull(instance); instance = this; // singleton logic

        Assert.IsNotNull(objToShow);

        Assert.IsNotNull(emitter);

        Show(false);
    }

    public void Show(bool show) {
        if(show)
            TitleScreenScript.instance.UpdateOptionsToggleText();
        objToShow.SetActive(show);
    }

    public void OnResumeButtonClick() {
        Show(false);
        GameStateManager.instance.OnPlay();

        var instance = emitter.EventInstance;
        instance.setPaused(false);
    }

    public void OnExitToTitleScreenButtonClick() {
        Show(false);
        GameStateManager.instance.OnShowTitleScreen();
    }

}
