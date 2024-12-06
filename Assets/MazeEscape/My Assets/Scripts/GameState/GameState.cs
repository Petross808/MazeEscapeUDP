using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField, EventSignature] GameEvent _onGameStartedEvent;
    [SerializeField, EventSignature] GameEvent _onMainMenuOpenedEvent;
    [SerializeField, EventSignature] GameEvent _onGameStateTransitionEvent;
    [SerializeField, EventSignature] GameEvent _onCheckpointSaveEvent;

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

    [EventSignature(typeof(GameObject))]
    public void CheckpointSave(GameEvent.CallbackContext _)
    {
        _onCheckpointSaveEvent.Raise(this);
    }

}
