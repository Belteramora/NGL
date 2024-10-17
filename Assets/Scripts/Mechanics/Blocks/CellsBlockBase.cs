using BreakInfinity;
using DG.Tweening;
using R3;
using UnityEditor.Build.Pipeline;
using UnityEditor.UIElements;
using UnityEngine;

public abstract class CellsBlockBase: BlockBase
{

    protected GameParameter<long> cellsBought;
    protected GameParameter<BigDouble> cellBaseValue;
    protected GameParameter<BigDouble> baseCost;
    protected GameParameter<BigDouble> costExponent;
    protected GameParameter<BigDouble> divider;

    public GameParameter<BigDouble> gainValue;
    public GameParameter<BigDouble> currentCost;

    public CellsBlockBase(GameData gameData, DepartmentBase parentDepartment): base(gameData, parentDepartment)
    {
    }

    public override void Setup()
    {

        base.Setup();
        cellsBought = blockData.Get<long>("cellsBought");

        //TODO: костыль, непонятно как разделить сетап дату с сейв датой
        cellsBought.needToSave = true;

        cellBaseValue = blockData.Get<BigDouble>("cellBaseValue");
        baseCost = blockData.Get<BigDouble>("cellBaseCost");
        costExponent = blockData.Get<BigDouble>("cellCostExponent");
        divider = blockData.Get<BigDouble>("cellDivider");

        var cost = CalculationHelper.CalculateCost(
            cellsBought.Value,
            baseCost.Value,
            costExponent.Value) / divider.Value;

        currentCost = blockData.Get<BigDouble>("currentCost");
        currentCost.SetFunction(() =>
        {
            return CalculationHelper.CalculateCost(
            cellsBought.Value,
            baseCost.Value,
            costExponent.Value) / divider.Value;
        });

        currentCost.Update();


        

        gainValue = blockData.Get<BigDouble>("gain");

    }
}
