using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Asteroids_ScoreManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    private int _currentScore;

    public static Asteroids_ScoreManager Instance;

    private void Awake() => Instance = this;

    private void Start()
    {
        Asteroids_UIManager.Instance.OnScreenChanged += (UIScreen screenChangedTo) =>
        {
            if (screenChangedTo == UIScreen.GAME_SCREEN)
                ResetScore();
        };
    }

    public void OnScored(IEnemy enemy, IEnemy[] additionalEnemiesSpwnd)
    {
        _currentScore += enemy.PointsGiven;
        ScoreText.SetText($"SCORE: {_currentScore.ToString("0")}");
    }

    private void ResetScore()
    {
        _currentScore = 0;
        ScoreText.SetText($"SCORE: {_currentScore.ToString("0")}");
    }
}