using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SaveData
{
    //Player
    public NavMeshAgent playerAgent;
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Transform playerHead;
    public int playerCurrentHealth;
    public bool playerAlive;

    //Enemies
    public NavMeshAgent enemyScarecrowAgent;
    public bool enemyScarecrowActivated;

    //Default values when game starts with no file
    public SaveData()
    {
        this.playerAgent = GameObject.Find("Player").GetComponent<NavMeshAgent>();
        this.playerPosition = playerAgent.nextPosition;
        this.playerHead = GameObject.Find("Head").GetComponent<Transform>();
        this.playerCurrentHealth = GameObject.Find("Hurtbox").GetComponent<HealthScript>().MaxHealth;
        this.playerAlive = true;
        this.enemyScarecrowAgent = GameObject.Find("ScarecrowEnemy").GetComponent<NavMeshAgent>();
        this.enemyScarecrowActivated = false;
    }
}
