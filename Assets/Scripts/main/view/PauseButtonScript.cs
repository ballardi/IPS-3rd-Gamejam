using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class PauseButtonScript : MonoBehaviour {
    

    public GameObject objToShow;

    public static PauseButtonScript instance;

    private void Awake() {
        Assert.IsNull(instance); instance = this; // singleton logic

        Assert.IsNotNull(objToShow);

        Show(false);
    }

    public void Show(bool show) {
        objToShow.SetActive(show);
    }


    public void OnPauseButtonClick() {
        GameStateManager.instance.OnPause();

       
    }

}
