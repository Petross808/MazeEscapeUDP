using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor.Rendering.HighDefinition;

public class GooScript : MonoBehaviour
{
    private int _duration = 0;
    private Material _material;

    private void Awake()
    {
        DecalProjector decalProjector = GetComponent<DecalProjector>();
        _material = new Material(decalProjector.material);
        decalProjector.material = _material;
    }

    public void Spawn(Vector3 position, int duration)
    {
        gameObject.SetActive(true);
        transform.position = position;
        _duration = duration;
        _material.SetFloat("_NoiseOffsetX", Random.Range(0f, 1000f));
        _material.SetFloat("_NoiseOffsetY", Random.Range(0f, 1000f));
    }

    [EventSignature]
    public void Tick(GameEvent.CallbackContext _)
    {
        _duration--;
        if (_duration <= 0)
            gameObject.SetActive(false);
    }
}
