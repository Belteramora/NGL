using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlacementBlockUI : BlockUI
{
    private GameParameter<long> gridSize;
    private Dictionary<string, GameParameter<string>> blockPoses;

    public RectTransform summaryContainer;
    public GameObject summaryPrefab;

    public RectTransform rootContainer;

    public GameObject gridPrefab;
    public RectTransform gridContainer;
    public GridElement[][] grid;
    public override void Setup(GameData gameData, DepartmentUI parentDepartment)
    {
        base.Setup(gameData, parentDepartment);

        Debug.Log("parent dep: " + parentDepartment.panelName + " panelName: " + panelName);
        gridSize = gameData.Parameters[parentDepartment.panelName][panelName].Get<long>("gridSize");

        



        //TODO: также переделать, хер пойми что написал
        grid = new GridElement[gridSize.Value][];

        for(int i = 0; i < gridSize.Value; i++)
        {
            grid[i] = new GridElement[gridSize.Value];

            for(int j = 0; j < gridSize.Value; j++)
            {
                var gridElementInstance = Instantiate(gridPrefab, gridContainer);
                grid[i][j] = gridElementInstance.GetComponent<GridElement>();
                grid[i][j].Setup(rootContainer, (i, j), ChangePosition);
            }
        }

        var blockPosesNotTyped = gameData.Parameters[parentDepartment.panelName][panelName].GetParametersByKeyMatchWithKey("BlockPos");
        blockPoses = new Dictionary<string, GameParameter<string>>();

        foreach (var key in blockPosesNotTyped.Keys)
        {
            blockPoses[key] = blockPosesNotTyped.Get<string>(key);
            var split = blockPoses[key].Value.Split(',');

            int i = int.Parse(split[0]);
            int j = int.Parse(split[1]);

            var blockName = key.Replace("Pos", string.Empty);
            var block = gameData.Parameters.GetBlock(blockName);
            grid[i][j].SetData(block, blockName);

            var summaryInstance = Instantiate(summaryPrefab, summaryContainer);
            var summary = summaryInstance.GetComponent<PBlockSummary>();

            summary.Setup(block.DisplayLetter, gameData.Parameters[parentDepartment.panelName][panelName].Get<BigDouble>(blockName));
        }
    }

    public void ChangePosition(DragElement el1, DragElement el2)
    {
        if (!el1.isEmpty && !el2.isEmpty)
        {
            var el1Data = gameData.Parameters.GetBlock(el1.appendedBlockName);
            var el1appBlockName = el1.appendedBlockName;

            var el2Data = gameData.Parameters.GetBlock(el2.appendedBlockName);
            var el2appBlockName = el2.appendedBlockName;

            grid[el1.index.Item1][el1.index.Item2].SetData(el2Data, el2appBlockName);
            grid[el2.index.Item1][el2.index.Item2].SetData(el1Data, el1appBlockName);

            blockPoses[el1appBlockName + "Pos"].BaseValue = el2.index.Item1 + "," + el2.index.Item2;
            blockPoses[el2appBlockName + "Pos"].BaseValue = el1.index.Item1 + "," + el1.index.Item2;
        }
        else if(el1.isEmpty && !el2.isEmpty)
        {
            var el2Data = gameData.Parameters.GetBlock(el2.appendedBlockName);
            var el2appBlockName = el2.appendedBlockName;

            grid[el1.index.Item1][el1.index.Item2].SetData(el2Data, el2appBlockName);
            grid[el2.index.Item1][el2.index.Item2].SetEmpty();

            blockPoses[el2appBlockName + "Pos"].BaseValue = el1.index.Item1 + "," + el1.index.Item2;
        }
        else if(!el1.isEmpty && el2.isEmpty)
        {
            var el1Data = gameData.Parameters.GetBlock(el1.appendedBlockName);
            var el1appBlockName = el1.appendedBlockName;

            grid[el1.index.Item1][el1.index.Item2].SetEmpty();
            grid[el2.index.Item1][el2.index.Item2].SetData(el1Data, el1appBlockName);

            blockPoses[el1appBlockName + "Pos"].BaseValue = el2.index.Item1 + "," + el2.index.Item2;
        }
    }


}
