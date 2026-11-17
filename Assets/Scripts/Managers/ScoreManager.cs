using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}
    public static event Action<int> ScoreChanged;
    public int score;
    void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }
    void OnDestroy() { if (Instance == this) Instance = null; }

    void OnEnable()
    {
        GameManager.StateChanged += OnStateChanged;
        EnemyLink.OnEnemyTouchedRing += AddScore;
    }
    void OnDisable()
    {
        GameManager.StateChanged -= OnStateChanged;
        EnemyLink.OnEnemyTouchedRing -= AddScore;
    }

    void OnStateChanged(GameState previous, GameState next)
    {
        if(next == GameState.PLAY && previous != GameState.PAUSED)
            score = 0;
    }
    public void AddScore(int points)
    {
        score += points;
        ScoreChanged?.Invoke(score);
    }
    public void ResetScore()
    {
        score = 0;
        ScoreChanged?.Invoke(score);
    }
    public int GetScore()
    {
        return score;
    }
}
