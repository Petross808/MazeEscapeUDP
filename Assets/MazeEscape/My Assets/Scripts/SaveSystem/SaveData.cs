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
    public Transform playerHead;
    public int playerCurrentHealth;
    public bool playerAlive;

    //Enemies
    public Vector3 enemyScarecrowPosition;
    public Quaternion enemyScarecrowRotation;
    public bool enemyScarecrowActivated;
    public SaveData() {}
}
