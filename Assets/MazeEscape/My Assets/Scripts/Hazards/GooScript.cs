using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class GooScript : MonoBehaviour
{
    private int _duration = 0;
    private Material _material;

    public int Duration { get => _duration; }

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
        transform.rotation = Quaternion.Euler(new(90, UnityEngine.Random.Range(0f, 360f), 0));
        _duration = duration;
        _material.SetFloat("_NoiseOffsetX", UnityEngine.Random.Range(0f, 1000f));
        _material.SetFloat("_NoiseOffsetY", UnityEngine.Random.Range(0f, 1000f));
    }

    [EventSignature]
    public void Tick(GameEvent.CallbackContext _)
    {
        _duration--;
        if (_duration <= 0)
            gameObject.SetActive(false);
    }

    [Serializable]
    public struct GooData
    {
        public Vector3 pos;
        public int dur;
    }
}
