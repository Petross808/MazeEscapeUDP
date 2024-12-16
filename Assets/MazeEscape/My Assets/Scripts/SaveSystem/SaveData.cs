using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SaveData
{
    //Player
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Quaternion playerHeadRotation;
    public int playerCurrentHealth;
    public bool playerAlive;

    //Enemies
    public Vector3 enemyScarecrowPosition;
    public Quaternion enemyScarecrowRotation;
    public bool enemyScarecrowActivated;
    public SaveData() {}

    public SaveData(SaveData other)
    {
        this.playerPosition = other.playerPosition;
        this.playerRotation = other.playerRotation;
        this.playerHeadRotation = other.playerHeadRotation;
        this.playerCurrentHealth = other.playerCurrentHealth;
        this.playerAlive = other.playerAlive;
        this.enemyScarecrowPosition = other.enemyScarecrowPosition;
        this.enemyScarecrowRotation = other.enemyScarecrowRotation;
        this.enemyScarecrowActivated = other.enemyScarecrowActivated;
    }
}
