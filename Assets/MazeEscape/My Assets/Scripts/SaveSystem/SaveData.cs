using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public Vector3 playerRotation;

    //Default values when game starts with no file
    public SaveData()
    {
        this.playerPosition = GameObject.Find("Player").transform.position;
        this.playerRotation = GameObject.Find("Player").transform.forward;
    }
}
