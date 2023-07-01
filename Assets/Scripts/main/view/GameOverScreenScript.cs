using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameOverScreenScript : MonoBehaviour
{
    public GameObject objToShow;

    public static GameOverScreenScript instance;

    private void Awake()
    {
        Assert.IsNull(instance); instance = this; // singleton logic

        Assert.IsNotNull(objToShow);

        Show(false);
    }

    public void Show(bool show)
    {
        objToShow.SetActive(show);
    }

    public void OnExitToTitleScreenButtonClick()
    {
        Show(false);
        GameStateManager.instance.OnShowTitleScreen();
    }

}
