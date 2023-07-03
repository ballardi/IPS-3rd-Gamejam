using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;

    [SerializeField]
    private Slider _scoreMetre;

    private bool _shouldDisplay;
    private int _currentScore;
    private int _destinationScore;
    private int _currentTimer;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_shouldDisplay)
        {
            if (_currentTimer == 0)
            {
                _currentTimer = 5;
                _scoreMetre.value = _currentScore;
                _scoreText.text = $"${_currentScore}";
                _currentScore++;

                if (_currentScore >= _destinationScore)
                    OnFinish();
            }
            else
                _currentTimer--;
        }
    }

    public void DisplayScore()
    {
        _currentScore = 0;
        _currentTimer = 5;
        _destinationScore = GameStateManager.instance.CurrentScore;
        _scoreMetre.value = 0;

        _shouldDisplay = true;
    }

    private void OnFinish()
    {
        _shouldDisplay = false;
    }
}
