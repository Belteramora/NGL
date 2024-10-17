using BreakInfinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BlockBase
{
    protected GameData gameData;
    protected RuntimeBlockData blockData;
    public string departmentName;
    public string name;

    protected GameParameter<BigDouble> blockCost;
    protected GameParameter<bool> isBought;

    public BlockBase(GameData gameData, DepartmentBase parentDepartment)
    {
        this.gameData = gameData;
        departmentName = parentDepartment.name;
        Setup();
    }

    public virtual void Setup()
    {
        blockData = gameData.Parameters[departmentName][name];

        isBought = blockData.IsBought;
        blockCost = blockData.Cost;
    }    
}
