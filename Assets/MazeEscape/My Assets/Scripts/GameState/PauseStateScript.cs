using UnityEngine;

public class PauseStateScript : MonoBehaviour
{
    private bool _isPaused;

    [SerializeField] private GameEvent _onPauseStateChangedEvent;

    public void TogglePause()
    {
        if (_isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (_isPaused)
            return;

        _isPaused = true;
        Time.timeScale = 0f;
        _onPauseStateChangedEvent.Raise(this, true);
    }

    public void ResumeGame()
    {
        if (!_isPaused)
            return;

        _isPaused = false;
        Time.timeScale = 1f;
        _onPauseStateChangedEvent.Raise(this, false);
    }
}
