using System;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}
    public static event Action<int> ScoreChanged;
    public int score;

    [SerializeField] private AudioSource scoreAudioSource;
    [SerializeField] private AudioClip onePointSound;
    [SerializeField] private AudioClip fivePointSound;
    [SerializeField] private AudioClip pointLossSound;

    void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }
    void OnDestroy() { if (Instance == this) Instance = null; }

    void OnEnable()
    {
        GameManager.StateChanged += OnStateChanged;
    }
    void OnDisable()
    {
        GameManager.StateChanged -= OnStateChanged;
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
        AudioClip awardClip = points switch
        {
            1 => onePointSound, 5 => fivePointSound, -1 => pointLossSound, _ => null
        };

        if (scoreAudioSource != null && awardClip != null)
        {
            scoreAudioSource.PlayOneShot(awardClip);
        }
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
