using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PauseMenuScript : MonoBehaviour {

    public GameObject objToShow;

    public static PauseMenuScript instance;

    private void Awake() {
        Assert.IsNull(instance); instance = this; // singleton logic

        Assert.IsNotNull(objToShow);

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
    }

    public void OnExitToTitleScreenButtonClick() {
        Show(false);
        GameStateManager.instance.OnShowTitleScreen();
    }

}
