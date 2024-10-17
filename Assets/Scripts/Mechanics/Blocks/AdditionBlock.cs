
using BreakInfinity;
using UnityEngine;

public class AdditionBlock: CellsBlockBase
{
    public AdditionBlock(GameData gameData, ProductionDepartment productionDepartment): base(gameData, productionDepartment)
    {
    }

    public override void Setup()
    {
        this.name = "ABlock";
        base.Setup();


        cellBaseValue.SetFunction(() =>
        {
            return cellBaseValue.BaseValue + gameData.Researches["R1"].TryGetValue(0) + gameData.Researches["R2"].TryGetValue(0);
        });

        gainValue.SetFunction(() =>
        {
            return cellBaseValue.Value * cellsBought.Value * gameData.Parameters["MngDep"]["PBlock"].Get<BigDouble>(name).Value;
        });
    }
}

