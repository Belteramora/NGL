
using R3;
using System;
using UnityEngine;
using Zenject;
using DG.Tweening;
using TMPro;
using BreakInfinity;

public class CellBlockUI: BlockUI
{
    protected IDisposable costTextDisposable;

    protected GameParameter<long> cellsCount;
    protected GameParameter<BigDouble> cellBaseValue;
    protected GameParameter<BigDouble> currentCost;
    protected GameParameter<BigDouble> gain;
    protected GameParameter<long> cellsBought;

    public GameObject blockCell;
    public RectTransform blockCellSpawnPoint;

    public TMP_Text summary;
    public TMP_Text costText;
    public string summaryText;

    public BlockCell[] cells;

    public override void Setup(GameData gameData, DepartmentUI parentDepartment)
    {
        base.Setup(gameData, parentDepartment); 
        //canBuy += block.CanBuy;
        //onBuy += block.OnBuyCell;        
    }

    private void Start()
    {
        cellsCount = block.Get<long>("cells");

        gain = block.Get<BigDouble>("gain");
        gain.SubscribeToModified((x) => summary.text = summaryText + x.ToString("G3"));

        cellsBought = block.Get<long>("cellsBought");
        cellBaseValue = block.Get<BigDouble>("cellBaseValue");
        currentCost = block.Get<BigDouble>("currentCost");
        currentCost.SubscribeToModified((x) => 
        {
            if (cellsCount.Value <= cellsBought.Value)
                costText.text = "MAX";
            else 
                costText.text = "Cost: " + x.ToString("G3") + " " + gameData.Currencies["main"].CurrencyShortName.Value;
        });

        cells = new BlockCell[cellsCount.Value];

        for (int i = 0; i < cellsCount.Value; i++)
        {
            cells[i] = Instantiate(blockCell, blockCellSpawnPoint).GetComponent<BlockCell>();
            cells[i].Setup(i, cellsBought, cellBaseValue);
        }
    }

    public bool CanBuy()
    {
        return gameData.Currencies["main"].CurrencyValue.Value >= currentCost.Value && cellsCount.Value > cellsBought.Value;
    }

    public void OnBuy()
    {
        if (CanBuy())
        {
            gameData.Currencies["main"].CurrencyValue.Value -= currentCost.Value;

            cellsBought.BaseValue++;

            currentCost.Update();
        }
    }
}
