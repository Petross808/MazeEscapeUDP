using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] GameEvent _onGameStartedEvent;
    [SerializeField] GameEvent _onMainMenuOpenedEvent;
    [SerializeField] GameEvent _onCheckpointSaveEvent;

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
        _onGameStartedEvent?.Raise(this);
    }

    [EventSignature]
    public void OpenMainMenu(GameEvent.CallbackContext _)
    {
        _onMainMenuOpenedEvent?.Raise(this);
    }

    [EventSignature(typeof(GameObject))]
    public void CheckpointSave(GameEvent.CallbackContext context)
    {
        _onCheckpointSaveEvent?.Raise(this);
    }

}
