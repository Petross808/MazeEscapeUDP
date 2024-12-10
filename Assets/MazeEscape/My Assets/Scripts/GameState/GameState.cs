using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField, EventSignature] GameEvent _onGameStartedEvent;
    [SerializeField, EventSignature] GameEvent _onMainMenuOpenedEvent;
    [SerializeField, EventSignature] GameEvent _onGameStateTransitionEvent;
    [SerializeField, EventSignature] GameEvent _onCheckpointSaveEvent;
    [SerializeField, EventSignature] GameEvent _onGameWonEvent;
    [SerializeField] GameObject _finalCheckpoint;

    private void Awake()
    {
        this.EnsureSingleInstance();
    }

    private void Start()
    {
        OpenMainMenu(new());
    }

    [EventSignature]
    public void PlayGame(GameEvent.CallbackContext _)
    {
        _onGameStateTransitionEvent.Raise(this);
        _onGameStartedEvent.Raise(this);
    }

    [EventSignature]
    public void OpenMainMenu(GameEvent.CallbackContext _)
    {
        _onGameStateTransitionEvent.Raise(this);
        _onMainMenuOpenedEvent.Raise(this);
    }

    [EventSignature]
    public void CheckpointSave(GameEvent.CallbackContext context)
    {
        _onCheckpointSaveEvent.Raise(this);

        if (context.Sender is Component go && go.gameObject == _finalCheckpoint)
            _onGameWonEvent.Raise(this);
    }

}
