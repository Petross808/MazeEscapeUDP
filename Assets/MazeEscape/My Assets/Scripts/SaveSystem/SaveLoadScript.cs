using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class SaveLoadScript : MonoBehaviour
{
    [SerializeField] List<Transform> _transformsToSave;

    private List<ISaveable> _savedData = new();

    private void Start()
    {
        foreach (var toSave in _transformsToSave)
        {
            _savedData.Add(new TransformSave(toSave));
        }

        HealthScript[] healthScripts = FindObjectsByType<HealthScript>(FindObjectsSortMode.None);
        foreach (var toSave in healthScripts)
        {
            _savedData.Add(new HealthSave(toSave));
        }

        NavMeshAgent[] agents = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);
        foreach (var toSave in agents)
        {
            _savedData.Add(new AgentSave(toSave));
        }

        ScarecrowAIScript[] scarecrowAi = FindObjectsByType<ScarecrowAIScript>(FindObjectsSortMode.None);
        foreach (var toSave in scarecrowAi)
        {
            _savedData.Add(new ScarecrowAISave(toSave));
        }
    }

    [EventSignature]
    public void Save(GameEvent.CallbackContext context)
    {
        foreach(var toSave in _savedData)
        {
            toSave.Save();
        }
    }

    [EventSignature]
    public void Load(GameEvent.CallbackContext context)
    {
        foreach(var toLoad in _savedData)
        {
            toLoad.Load();
        }
    }

    [EventSignature]
    public void ResetInit(GameEvent.CallbackContext context)
    {
        foreach (var toReset in _savedData)
        {
            toReset.ResetInit();
        }
    }


    interface ISaveable
    {
        public void Save();
        public void Load();
        public void ResetInit();
    }

    struct TransformSave : ISaveable
    {
        public Transform SavedObject;
        public Vector3 SavedPosition;
        public Vector3 InitialPosition;
        public Quaternion SavedRotation;
        public Quaternion InitialRotation;

        public TransformSave(Transform toSave)
        {
            SavedObject = toSave;
            InitialPosition = toSave.transform.position;
            InitialRotation = toSave.transform.rotation;
            SavedPosition = toSave.transform.position;
            SavedRotation = toSave.transform.rotation;
        }

        public void Save()
        {
            SavedPosition = SavedObject.transform.position;
            SavedRotation = SavedObject.transform.rotation;
        }

        public void Load()
        {
            SavedObject.transform.position = SavedPosition;
            SavedObject.transform.rotation = SavedRotation;
        }

        public void ResetInit()
        {
            SavedPosition = InitialPosition;
            SavedRotation = InitialRotation;
            Load();
        }

    }

    struct HealthSave : ISaveable
    {
        public HealthScript SavedScript;
        public int SavedHealth;
        public int InitalHealth;

        public HealthSave(HealthScript toSave)
        {
            SavedScript = toSave;
            InitalHealth = toSave.CurrentHealth;
            SavedHealth = toSave.CurrentHealth;
        }

        public void Save() => SavedHealth = SavedScript.CurrentHealth;
        public void Load() => SavedScript.SetRawHealth(SavedHealth);
        public void ResetInit()
        {
            SavedHealth = InitalHealth;
            Load();
        }
    }

    struct AgentSave : ISaveable
    {
        public NavMeshAgent SavedAgent;
        public Vector3 InitialPosition;
        public Vector3 SavedPosition;
        public Quaternion InitialRotation;
        public Quaternion SavedRotation;

        public AgentSave(NavMeshAgent toSave)
        {
            SavedAgent = toSave;
            InitialPosition = SavedAgent.nextPosition;
            SavedPosition = SavedAgent.nextPosition;
            InitialRotation = SavedAgent.gameObject.transform.rotation;
            SavedRotation = SavedAgent.gameObject.transform.rotation;
        }

        public void Save()
        {
            SavedPosition = SavedAgent.nextPosition;
            SavedRotation = SavedAgent.gameObject.transform.rotation;
        }

        public void Load()
        {
            SavedAgent.Warp(SavedPosition);
            SavedAgent.gameObject.transform.rotation = SavedRotation;
        }
        public void ResetInit()
        {
            SavedPosition = InitialPosition;
            SavedRotation = InitialRotation;
            Load();
        }
    }

    struct ScarecrowAISave : ISaveable
    {
        public ScarecrowAIScript SavedScript;
        public bool SavedActive;
        public bool InitialActive;

        public ScarecrowAISave(ScarecrowAIScript toSave)
        {
            SavedScript = toSave;
            InitialActive = toSave.Activated;
            SavedActive = toSave.Activated;
        }

        public void Save() => SavedActive = SavedScript.Activated;
        public void Load() => SavedScript.Activated = SavedActive;
        public void ResetInit()
        {
            SavedActive = InitialActive;
            Load();
        }
    }
}
