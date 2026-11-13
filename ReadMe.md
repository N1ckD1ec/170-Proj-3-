## GameManager Calls
GameManager currently determines the state of the game, whether it is the Title, Paused, Play, or Gameover. To call a state change, the necessary function shall call:
- `GameManager.Instance.StartGame();`
- `GameManager.Instance.Pause();`
- `GameManager.Instance.Resume();`
- `GameManager.Instance.GameOver();`
- `GameManager.Instance.RestartGame();`
- `GameManager.Instance.ReturnToTitle();`
Any scripts that rely on new information based off the state change, aka UI or gameplay elements, should implement the following lines:
```c#
void OnEnable()  => GameManager.StateChanged += OnStateChanged;
void OnDisable() => GameManager.StateChanged -= OnStateChanged;

void OnStateChanged(GameState previous, GameState next)
{
    //necessary listeners
}
```
Alternatively, using polling could also be acceptable here.
```c#
void Update()
{
    if (GameManager.Instance.Current != GameState.PLAY) return;
    // gameplay logic
}
```