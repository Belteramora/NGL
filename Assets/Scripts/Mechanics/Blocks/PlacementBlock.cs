using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class PlacementBlock : BlockBase
{
    private Dictionary<string, RuntimeBlockData> gainBlocks;
    public GameParameter<bool> needRecalculate;
    public GameParameter<long> gridSize;
    public string[][] grid;

    public PlacementBlock(GameData gameData, DepartmentBase parentDepartment) : base(gameData, parentDepartment)
    {
    }

    //TODO: наворотил тут жесткой срани в сетапе
    public override void Setup()
    {
        name = "PBlock";
        base.Setup();

        gainBlocks = GetAviableBlocks();

        needRecalculate = blockData.Get<bool>("needRecalculateGraph");
        gridSize = blockData.Get<long>("gridSize");

        Debug.Log("GET GRID SIZE IN PBLOCK SETUP");

        needRecalculate.needToSave = true;


        if (needRecalculate.Value)
        {
            needRecalculate.BaseValue = false;
            //TODO: тут мув с очисткой словаря и добавлением параметра (ов) которые удалять не нужно было. ПОПРАВИТЬ!!!


            foreach (var block in gainBlocks)
            {

                foreach (var block2 in gainBlocks)
                {
                    if (block.Key == block2.Key)
                        continue;

                    string parameterName = block.Key + "-" + block2.Key;

                    blockData[parameterName] = new GameParameter<BigDouble>(Random.Range(1f, 2f), true);
                }
            }
        }

        grid = new string[gridSize.Value][];

        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new string[gridSize.Value];

            for(int j = 0; j < gridSize.Value; j++)
            {
                grid[i][j] = string.Empty;
            }
        }

        var blockPoses = blockData.GetParametersByKeyMatchWithKey("BlockPos");

        if(blockPoses.Count == 0)
        {
            SetDefault();
        }
        else
        {
            SetByPoses(blockPoses);
        }

        foreach(var block in gainBlocks)
        {
            blockData.Get<string>(block.Key + "Pos").SubscribeToBase(x => SetPosition(block.Key + "Pos", x));
        }

        Calculate();

        string pos = string.Empty;

        for(int i = 0; i < gridSize.Value; i++)
        {
            for(int j = 0;j < gridSize.Value; j++)
            {
                pos += grid[i][j] + "\t";
            }
            pos += "\n";
        }

        Debug.Log("Pos is:\n" + pos);


        foreach(var block in gainBlocks)
        {
            var blockGain = blockData.Get<BigDouble>(block.Key);
            blockGain.SetFunction(() =>
            {
                if (!gameData.Parameters[departmentName].IsBought.Value && !blockData.IsBought.Value)
                    return 1;

                return blockGain.BaseValue;
            });
        }
    }

    public void SetPosition(string blockName, string position)
    {
        Debug.Log("Change position of " + blockName + " position");

        for(int i = 0; i < gridSize.Value; i++)
        {
            for(int j = 0; j < gridSize.Value; j++)
            {
                if(grid[i][j] == blockName)
                    grid[i][j] = string.Empty;
            }
        }

        var split = position.Split(',');

        int indexI = int.Parse(split[0]);
        int indexJ = int.Parse(split[1]);

        grid[indexI][indexJ] = blockName;

        Calculate();
    }

    //TODO: обязательно доработать, сейчас тут самый простой вариант
    //Алгоритм расчета отношений
    public void Calculate()
    {
        var gains = new Dictionary<string, GameParameter<BigDouble>>();
        foreach(var key in gainBlocks.Keys)
        {
            gains[key] = blockData.Get<BigDouble>(key);
            gains[key].BaseValue = 1;
        }

        Debug.Log("Calc call");

        for(int i = 0; i < gridSize.Value; i++)
        {
            for(int j = 0; j < gridSize.Value; j++)
            {
                if (string.IsNullOrEmpty(grid[i][j])) continue;

                if(i + 1 < gridSize.Value)
                {
                    if (!string.IsNullOrEmpty(grid[i + 1][j]))
                    {
                        var a = grid[i][j].Replace("Pos", string.Empty);
                        var b = grid[i + 1][j].Replace("Pos", string.Empty);
                        var gainAtB = blockData.Get<BigDouble>(a + "-" + b);
                        var gainBtA = blockData.Get<BigDouble>(b + "-" + a);

                        gains[a].BaseValue *= gainAtB.Value;
                        gains[b].BaseValue *= gainBtA.Value;
                    }
                }

                if (j + 1 < gridSize.Value)
                {
                    if (!string.IsNullOrEmpty(grid[i][j + 1]))
                    {
                        var a = grid[i][j].Replace("Pos", string.Empty);
                        var b = grid[i][j + 1].Replace("Pos", string.Empty);
                        var gainAtB = blockData.Get<BigDouble>(a + "-" + b);
                        var gainBtA = blockData.Get<BigDouble>(b + "-" + a);

                        gains[a].BaseValue *= gainAtB.Value;
                        gains[b].BaseValue *= gainBtA.Value;
                    }
                }
            }
        }
    }

    public Dictionary<string, RuntimeBlockData> GetAviableBlocks()
    {
        return gameData.Parameters.departments.Where(c => c.Value.IsBought.Value).
            Select(c => c.Value).
            SelectMany(c => c.blocks).
            Where(c => c.Value.IsBought.Value && c.Value.ContainsParameter("gain")).
            ToDictionary(x => x.Key, c => c.Value);
    }

    public void SetDefault()
    {
        foreach(var block in gainBlocks)
        {
            var emptyPos = GetFirstEmptyCell();

            if (emptyPos.Item1 == -1 || emptyPos.Item2 == -1) {
                Debug.LogError("GRAPH IS MAXED");
                return;
            }
            blockData[block.Key + "Pos"] = new GameParameter<string>(emptyPos.Item1 + "," + emptyPos.Item2, true);
            grid[emptyPos.Item1][emptyPos.Item2] = block.Key + "Pos";
        }
    }

    public void SetByPoses(Dictionary<string, GameParameter> keyWithPoses)
    {
        Debug.Log("Set by poses");
        foreach(var keyWithPos in keyWithPoses)
        {
            var split = (keyWithPos.Value as GameParameter<string>).Value.Split(',');

            int i = int.Parse(split[0]);
            int j = int.Parse(split[1]);

            grid[i][j] = keyWithPos.Key;
        }
    }

    public (int, int) GetFirstEmptyCell()
    {
        for (int i = 0; i < grid.Length; i++)
        {
            for(int j = 0;  j < grid[i].Length; j++)
            {
                if(string.IsNullOrEmpty(grid[i][j]))
                    return (i, j);
            }
        }

        return (-1, -1);
    }
}
