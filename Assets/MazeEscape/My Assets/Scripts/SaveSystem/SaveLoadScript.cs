using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SaveLoadScript : MonoBehaviour
{
    private SaveData _saveData;
    private SaveData _initialSaveData;
    private List<ISaveData> _saveDataObjects;

    [Header("File Storage Name")]
    [SerializeField] private string _fileName;
    private FileHandler _fileHandler;

    private void Start()
    {
        this.EnsureSingleInstance();

        this._saveDataObjects = FindAllSaveDataObjects();
        this._fileHandler = new FileHandler(Application.persistentDataPath, _fileName);
        this._saveData = new SaveData();
        SaveGame(new());
        this._initialSaveData = new SaveData(_saveData);
    }

    [EventSignature]
    public void NewGame(GameEvent.CallbackContext context)
    {
        this._fileHandler.Save(_initialSaveData);
    }

    [EventSignature]
    public void LoadGame(GameEvent.CallbackContext context)
    {
        if(this._saveData == null)
        {
            NewGame(context);
        }
        else if(this._fileHandler.SaveExists())
        {
            this._saveData = this._fileHandler.Load();
        }

        foreach(ISaveData saveDataObj in  this._saveDataObjects)
        {
            saveDataObj.LoadData(_saveData);
        }
    }

    [EventSignature]
    public void SaveGame(GameEvent.CallbackContext context)
    {
        foreach(ISaveData saveDataObj in this._saveDataObjects)
        {
            saveDataObj.SaveData(ref _saveData);
        }

        this._fileHandler.Save(_saveData);
    }

    private List<ISaveData> FindAllSaveDataObjects()
    {
        IEnumerable<ISaveData> saveDataObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveData>();       
        return new List<ISaveData>(saveDataObjects);
    }
}
