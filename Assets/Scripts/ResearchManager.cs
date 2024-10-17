using Zenject;
using BreakInfinity;
using UnityEngine;

public class ResearchManager: IInitializable
{
    private GameData gameData;

    public ResearchManager(GameData gameData)
    {
        this.gameData = gameData;

    }

    public void Initialize()
    {
        gameData.Researches["R1"].Effect.SetFunction(R1);
        gameData.Researches["R2"].Effect.SetFunction(R2);
        gameData.Researches["R3"].Effect.SetFunction(R3);
        gameData.Researches["R4"].Effect.SetFunction(R4);
        gameData.Researches["R5"].Effect.SetFunction(R5);
        gameData.Researches["R6"].Effect.SetFunction(R6);
    }

    public BigDouble R1()
    {
        
        return 1;
    }

    public BigDouble R2()
    {
        var value1 = gameData.Parameters["PDep"]["ABlock"].Get<long>("cellsBought").Value;

        return value1 * 0.1f;
    }

    public BigDouble R3()
    {
        return 1;
    }

    public BigDouble R4()
    {
        long boughtSum = 0;

        var resultList = gameData.Parameters.GetParametersByKeyMatch("cellsBought");
        foreach(var param in resultList)
        {
            boughtSum += (param as GameParameter<long>).Value;
        }

        return boughtSum * 0.1f;
    }

    public BigDouble R5()
    {
        return 0.5f;
    }

    public BigDouble R6()
    {
        long boughtSum = 0;

        var resultList = gameData.Parameters.GetParametersByKeyMatch("cellsBought");
        foreach (var param in resultList)
        {
            boughtSum += (param as GameParameter<long>).Value;
        }

        return boughtSum * 0.01f;
    }
}

