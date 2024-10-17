
using BreakInfinity;
using R3;
using UnityEngine;
using Zenject;

public class MultiplicationBlock : CellsBlockBase
{
    public MultiplicationBlock(GameData gameData, ProductionDepartment parentDepartment): base(gameData, parentDepartment)
    {
    }

    public override void Setup()
    {
        name = "MBlock";

        base.Setup();

        cellBaseValue.SetFunction(() =>
        {
            return cellBaseValue.BaseValue + gameData.Researches["R5"].TryGetValue(0);
        });

        gainValue.SetFunction(() =>
        {
            if (cellsBought.Value == 0) return 1;

            return BigDouble.Pow(cellBaseValue.Value * gameData.Parameters["MngDep"]["PBlock"].Get<BigDouble>(name).Value, cellsBought.Value);
        });
    }
}

