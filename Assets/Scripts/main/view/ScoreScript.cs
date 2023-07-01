using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ScoreScript : MonoBehaviour {

    public TextMeshProUGUI ScoreText;

    private void Awake() {
        Assert.IsNotNull(ScoreText);
    }

    // Update is called once per frame
    void Update() {
        if (GameStateManager.instance.CurrentState == GameStateManager.STATE.PLAYING) {
            ScoreText.text = "Score: " + GameStateManager.instance.CurrentScore;
        }
    }
}
