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
                        saveFromLoad = reader.ReadToEnd();
                    }
                }
                save = JsonUtility.FromJson<SaveData>(saveFromLoad);
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
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(this._path));

            string saveToWrite = JsonUtility.ToJson(save, true);

            using(FileStream stream = new FileStream(this._path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(saveToWrite);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }
}