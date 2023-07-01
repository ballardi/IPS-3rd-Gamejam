using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TitleScreenScript : MonoBehaviour {

    public GameObject objToShow;

    public static TitleScreenScript instance;

    private void Awake() {
        Assert.IsNull(instance); instance = this; // singleton logic

        Assert.IsNotNull(objToShow);
    }

    public void Show(bool show) {
        objToShow.SetActive(show);
    }

    public void OnPlayButtonClick() {
        Show(false);
        GameStateManager.instance.OnPlay();
    }

}
