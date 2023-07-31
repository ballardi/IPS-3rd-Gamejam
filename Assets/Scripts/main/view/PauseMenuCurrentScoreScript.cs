using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;

public class PauseMenuCurrentScoreScript : MonoBehaviour
{
    public TextMeshProUGUI textToUpdate;

    private void Awake() {
        Assert.IsNotNull(textToUpdate);
    }

    void Update() {
        textToUpdate.text = "" + GameStateManager.instance.CurrentScore;
    }
}
