using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}
    public int score;
    void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }
    void OnDestroy() { if (Instance == this) Instance = null; }

    void OnEnable()  => GameManager.StateChanged += OnStateChanged;
    void OnDisable() => GameManager.StateChanged -= OnStateChanged;

    void OnStateChanged(GameState previous, GameState next)
    {
        if(next == GameState.PLAY)
            score = 0;
    }
}
