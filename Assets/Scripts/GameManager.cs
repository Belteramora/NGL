using BreakInfinity;
using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using Zenject;

public class GameManager : ITickable
{
    private ProductionDepartment productionDepartment;
    private MarketingDepartment marketingDepartment;
    private ResearchDepartment researchDepartment;
    private ManagementDepartment managementDepartment;

    private GameData gameData;

    public GameManager(GameData gameData)
    {
        productionDepartment = new ProductionDepartment(gameData);
        marketingDepartment = new MarketingDepartment(gameData);
        researchDepartment = new ResearchDepartment(gameData);
        managementDepartment = new ManagementDepartment(gameData);

        this.gameData = gameData;

        gameData.Currencies["things"].CurrencyIncreaseValue.SetFunction(() => productionDepartment.gainValue.Value);

        gameData.Currencies["main"].CurrencyIncreaseValue.SetFunction(() =>
        {
            var thingsTakeCount = new BigDouble(0);

            if (gameData.Currencies["things"].CurrencyValue.Value <= 0)
                thingsTakeCount = 0;
            else if (gameData.Currencies["things"].CurrencyValue.Value < marketingDepartment.gainValue.Value)
                thingsTakeCount = gameData.Currencies["things"].CurrencyValue.Value;
            else
                thingsTakeCount = marketingDepartment.gainValue.Value;

            return thingsTakeCount;
        });

        gameData.UpdateAll();
    }

    float interval = 1;
    float timer = 0;

    public void Tick()
    {
        foreach(var res in gameData.Researches.Values)
        {
            res.Update();
        }

        timer += Time.deltaTime;    
        if(timer > interval)
        {
            timer = 0;

            TickPerformed();
        }
    }

    public void TickPerformed()
    {
        
        gameData.Currencies["things"].CurrencyValue.Value += gameData.Currencies["things"].CurrencyIncreaseValue.Value;

        gameData.Currencies["main"].CurrencyValue.Value += gameData.Currencies["main"].CurrencyIncreaseValue.Value;

        gameData.Currencies["things"].CurrencyValue.Value -= gameData.Currencies["main"].CurrencyIncreaseValue.Value;
    }
}
