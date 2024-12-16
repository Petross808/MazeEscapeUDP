using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All objects necessary to be saved have to implement this interface
public interface ISaveData
{
    void LoadData(SaveData data);
    void SaveData(ref SaveData data);
}