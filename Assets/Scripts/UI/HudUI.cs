using TMPro;
using UnityEngine;

public class HudUI : MonoBehaviour
{
    
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TextMeshProUGUI scoreText;
 
    void OnEnable()
    {
        ScoreManager.ScoreChanged += HandleScoreUpdate;
    }

    void OnDisable()
    {
        ScoreManager.ScoreChanged -= HandleScoreUpdate;
    }
    
    void Start()
    {
        int initialScore;
        if (scoreManager != null)
            initialScore = scoreManager.GetScore();
        else if (ScoreManager.Instance != null)
            initialScore = ScoreManager.Instance.GetScore();
        else
            initialScore = 0;
        
        HandleScoreUpdate(initialScore);
    }

    void HandleScoreUpdate(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        } 
    }
}
