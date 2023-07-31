using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PauseMenuHighScoreScript : MonoBehaviour
{
    public TextMeshProUGUI textToUpdate;

    private void Awake() {
        Assert.IsNotNull(textToUpdate);
    }

    void Update() {
        textToUpdate.text = "" + GameStateManager.instance.HighScore;
    }
}
