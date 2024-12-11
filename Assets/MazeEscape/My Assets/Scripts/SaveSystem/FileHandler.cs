using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class FileHandler
{
    private string _path = "";

    public FileHandler(string dataPath, string fileName)
    {
        this._path = Path.Combine(dataPath, fileName);
    }

    public bool SaveExists()
    {
        return File.Exists(this._path);
    }

    public SaveData Load()
    {
        Debug.Log("Attempt to load from file");
        SaveData save = null;
        if (File.Exists(this._path))
        {
            try
            {
                string saveFromLoad = "";
                using (FileStream stream = new FileStream(this._path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        saveFromLoad = reader.ToString();
                    }

                }
                save = JsonUtility.FromJson<SaveData>(saveFromLoad);
                Debug.Log("Succesfully loaded");
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
        return save;
    }

    public void Save(SaveData save)
    {
        Debug.Log("Attempt to savet to file");
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(this._path));

            string saveToWrite = JsonUtility.ToJson(save);

            using(FileStream stream = new FileStream(this._path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(saveToWrite);
                }
            }
            Debug.Log("Succesfully saved?");
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }
}