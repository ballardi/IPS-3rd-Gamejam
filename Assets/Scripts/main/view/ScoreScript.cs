using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ScoreScript : MonoBehaviour {

    public static ScoreScript instance;

    public GameObject objToShow;
    public TextMeshProUGUI ScoreText;

    private void Awake() {
        Assert.IsNull(instance); instance = this;

        Assert.IsNotNull(ScoreText);
        Assert.IsNotNull(objToShow);
    }

    // Update is called once per frame
    void Update() {
        if (GameStateManager.instance.CurrentState == GameStateManager.STATE.PLAYING) {
            ScoreText.text = "" + GameStateManager.instance.CurrentScore;
        }
    }

    public void Show(bool show) {
        objToShow.SetActive(show);
    }
}
