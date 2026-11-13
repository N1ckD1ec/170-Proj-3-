using System;
using UnityEngine;

public enum GameState
{
    TITLE,
    PLAY,
    PAUSED,
    GAMEOVER
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public GameState gameState {get; private set;} = GameState.TITLE;

    public static event Action<GameState, GameState> StateChanged;

    void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }
    void OnDestroy() { if (Instance == this) Instance = null; }
    public void StartGame() {
        SafeStateChange(GameState.TITLE, GameState.PLAY);
    }
    public void Pause() {
        SafeStateChange(GameState.PLAY, GameState.PAUSED);
    }
    public void Resume() {
        SafeStateChange(GameState.PAUSED, GameState.PLAY);
    }
    public void GameOver() {
        SafeStateChange(GameState.PLAY, GameState.GAMEOVER);
    }
    public void RestartGame() {
        SafeStateChange(GameState.GAMEOVER, GameState.PLAY);
    }
    public void ReturnToTitle() { //this is the one with a flexible previous state
        if(gameState == GameState.TITLE) return;
        SafeStateChange(gameState, GameState.TITLE);
    }

    void SafeStateChange(GameState prev, GameState next) {
        if(gameState == prev)
            gameState = next;
        else
        {
            Debug.LogWarning("Couldn't change state to " + next + " as current state " + gameState + " was not state " + prev, this);
            return;
        }
        StateChanged?.Invoke(prev, next);
    }
}