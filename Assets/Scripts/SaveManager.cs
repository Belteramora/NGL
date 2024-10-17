using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;

//TODO: вышло немного мешанины и пересечений с GameData, т.к. SaveData нужна в GameData, а GameData нужна в SaveData
public class SaveManager: ITickable
{
    public static string savePath = "C:\\Users\\Administrator\\Documents\\save.json";

    private float saveTimeInterval = 5f;
    private float timer;

    private GameData gameData;

    
    public SaveManager(GameData gameData)
    {
        this.gameData = gameData;
    }

    public void Save()
    {

        SaveData saveData = new SaveData(gameData);


        string jsonData = JsonConvert.SerializeObject(saveData, Formatting.Indented);

        File.WriteAllText(savePath, jsonData);
    }


    public void Tick()
    {
        timer += Time.deltaTime;

        if (timer > saveTimeInterval)
        {

            timer = 0;

            Save();
            Debug.Log("Save performed");
        }
    }
}


