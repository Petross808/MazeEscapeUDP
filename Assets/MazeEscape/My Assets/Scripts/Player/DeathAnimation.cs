using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private float _turnTime;
    [SerializeField] private float _holdTime;
    [SerializeField, EventSignature] private GameEvent _onDeathAnimationStart;
    [SerializeField, EventSignature] private GameEvent _onDeathAnimationEnd;
    [SerializeField, EventSignature] private GameEvent _onPlayerRespawn;


    [EventSignature(typeof(GameObject))]
    public void DeathSequence(GameEvent.CallbackContext context)
    {
        StartCoroutine(DeathCoroutine(context.Get<GameObject>()));
    }

    private IEnumerator DeathCoroutine(GameObject diedTo)
    {
        _onDeathAnimationStart.Raise(this);
        Vector3 start = _head.forward;
        Vector3 target = diedTo.transform.position - _head.position;
        diedTo.transform.root.transform.forward = _head.position - diedTo.transform.root.transform.position;

        float elapsed = 0;
        while(elapsed < _turnTime)
        {
            elapsed += Time.deltaTime;
            _head.forward = Vector3.Slerp(start, target, elapsed/_turnTime);
            yield return null;
        }
        yield return new WaitForSeconds(_holdTime/2);
        _onDeathAnimationEnd.Raise(this);
        yield return new WaitForSeconds(_holdTime / 2);
        _onPlayerRespawn.Raise(this);
    }
}
