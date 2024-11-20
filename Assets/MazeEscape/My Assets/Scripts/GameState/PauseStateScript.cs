using UnityEngine;

public class PauseStateScript : MonoBehaviour
{
    [SerializeField] private bool _isPaused;
    [SerializeField] private GameEvent _onPauseStateChangedEvent;

    private void Awake()
    {
        this.EnsureSingleInstance();
    }

    private void Start()
    {
        _onPauseStateChangedEvent.Raise(this, _isPaused);
    }

    [EventSignature(typeof(bool))]
    public void SetPause(GameEvent.CallbackContext context)
    {
        bool newPauseState = context.Get<bool>();
        if (newPauseState)
            PauseGame(context);
        else
            ResumeGame(context);
    }

    [EventSignature]
    public void PauseGame(GameEvent.CallbackContext context)
    {
        if (_isPaused)
            return;

        _isPaused = true;
        Time.timeScale = 0f;
        _onPauseStateChangedEvent.Raise(this, true);
    }

    [EventSignature]
    public void ResumeGame(GameEvent.CallbackContext context)
    {
        if (!_isPaused)
            return;

        _isPaused = false;
        Time.timeScale = 1f;
        _onPauseStateChangedEvent.Raise(this, false);
    }
}
