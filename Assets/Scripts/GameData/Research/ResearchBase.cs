
using BreakInfinity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class ResearchBase: IUpdatable
{
    protected GameData gameData;
    public GameParameter<bool> IsBought { get; set; }
    public string name;
    public string displayName;
    public string description;
    public string attachedDepartment;
    public string attachedBlock;
    public string effectSign;

    public GameParameter<BigDouble> BaseCost { get; set; }
    public GameParameter<BigDouble> Effect { get; set; }

    public ResearchBase(GameData gameData, ResearchSetup setup)
    {
        this.gameData = gameData;

        this.description = setup.description;
        this.displayName = setup.displayName;
        this.effectSign = setup.effectSign;

        this.attachedDepartment = setup.attachedDepartment;
        this.attachedBlock = setup.attachedBlock;

        //TODO: возможно тоже добавить эти параметры в реестр
        IsBought = new GameParameter<bool>(false, true);
        BaseCost = new GameParameter<BigDouble>(setup.baseCost, false);
        Effect = new GameParameter<BigDouble>(1, false);
    }

    public BigDouble TryGetValue(BigDouble notBoughtValue)
    {
        if (!IsBought.Value) return notBoughtValue;
        
        return Effect.Value;
    }

    public bool IsAvailable()
    {
        if (!string.IsNullOrEmpty(attachedBlock) && !string.IsNullOrEmpty(attachedDepartment))
            return gameData.Parameters[attachedDepartment].IsBought.Value && gameData.Parameters[attachedDepartment][attachedBlock].IsBought.Value;
        else if (!string.IsNullOrEmpty(attachedDepartment))
            return gameData.Parameters[attachedDepartment].IsBought.Value;
        else
            return true;
    }

    public void Update()
    {
        IsBought.Update();
        Effect.Update();
        BaseCost.Update();
    }
}

