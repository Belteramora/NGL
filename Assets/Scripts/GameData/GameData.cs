using BreakInfinity;
using Newtonsoft.Json;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Zenject;

public class GameData
{
    public RuntimeData Parameters { get; private set; }

    public Dictionary<string, Currency> Currencies { get; private set; }
    public Dictionary<string, ResearchBase> Researches { get; private set; }

    public GameData(TextAsset configText)
    {
        var config = JsonConvert.DeserializeObject<ConfigData>(configText.text);
        
        Configure(config);

        var saveData = GetSaveData();

        if (saveData != null)
        {
            Load(saveData);
        }

        Logger.Log(Channel.Loading, "dataPath is " + Application.dataPath +  "\npersistentDataPath is "  + Application.persistentDataPath);
    }

    public void Configure(ConfigData config)
    {
        this.Currencies = new Dictionary<string, Currency>();
        this.Parameters = new RuntimeData();
        this.Researches = new Dictionary<string, ResearchBase>();

        foreach (var currencyData in config.currencySetups)
        {
            this.Currencies.Add(currencyData.Key, new Currency(currencyData.Value));
        }

        foreach (var department in config.departments)
        {
            var departmentData = new RuntimeDepartmentData(department.Value);

            Parameters[department.Key] = departmentData;

        }

        foreach (var researchData in config.researches)
        {
            this.Researches.Add(researchData.Key, new ResearchBase(this, researchData.Value));
        }
    }

    public SaveData GetSaveData()
    {
        if (!File.Exists(SaveManager.savePath)) return null;

        var saveDataString = File.ReadAllText(SaveManager.savePath);
        var saveData = JsonConvert.DeserializeObject<SaveData>(saveDataString);

        return saveData;
    }

    public void Load(SaveData saveData)
    {
        foreach (var currencyData in saveData.currencies)
        {
            Currencies[currencyData.Key].CurrencyValue.Value = BigDouble.Parse(currencyData.Value);
        }

        foreach (var department in saveData.departments)
        {
            Parameters[department.Key].IsBought.BaseValue = department.Value.isBought;
            Helper.InjectSaveData(ref Parameters[department.Key].parameters, department.Value.parameters);

            foreach (var saveBlockData in department.Value.saveBlocks)
            {
                Parameters[department.Key][saveBlockData.Key].IsBought.BaseValue = saveBlockData.Value.isBought;

                Helper.InjectSaveData(ref Parameters[department.Key][saveBlockData.Key].parameters, saveBlockData.Value.parameters);
            }

        }

        foreach (var researchData in saveData.researches)
        {
            this.Researches[researchData.Key].IsBought.BaseValue = researchData.Value;
        }
    }

    public void UpdateAll()
    {
        foreach(var res in Researches.Values)
        {
            res.Update();
        }

        foreach(var currency in Currencies.Values)
        {
            currency.CurrencyIncreaseValue.Update();
        }

        Parameters.Update();
    }
}
