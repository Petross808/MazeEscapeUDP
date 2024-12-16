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

    //Scarecrow
    public Vector3 enemyScarecrowPosition;
    public Quaternion enemyScarecrowRotation;
    public bool enemyScarecrowActivated;

    //Blob
    public int blob_currentWaypointIndex;
    public int blob_chaseTimer;
    public int blob_cooldownTimer;
    public int blob_wanderTimer;
    public bool blob_isChasingPlayer;
    public bool blob_isInCooldown;
    public Vector3 blob_position;
    public Quaternion blob_rotation;
    public bool blob_isStopped;
    public int blob_currentGooIndex;
    public List<GooScript.GooData> blob_gooTrail;

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

        //Blob
        this.blob_currentWaypointIndex = other.blob_currentWaypointIndex;
        this.blob_chaseTimer = other.blob_chaseTimer;
        this.blob_cooldownTimer = other.blob_cooldownTimer;
        this.blob_wanderTimer = other.blob_wanderTimer;
        this.blob_isChasingPlayer = other.blob_isChasingPlayer;
        this.blob_isInCooldown = other.blob_isInCooldown;
        this.blob_position = other.blob_position;
        this.blob_rotation = other.blob_rotation;
        this.blob_isStopped = other.blob_isStopped;
        this.blob_currentGooIndex = other.blob_currentGooIndex;
        this.blob_gooTrail = new(other.blob_gooTrail);

    }
}
