using System.Collections.Generic;

public class SaveData
{
    public Dictionary<string, string> currencies;
    public Dictionary<string, SaveDepartment> departments;
    public Dictionary<string, bool> researches;

    public SaveData()
    {
        currencies = new Dictionary<string, string>();
        researches = new Dictionary<string, bool>();
        departments = new Dictionary<string, SaveDepartment>();
    }

    public SaveData(GameData gameData)
    {
        currencies = new Dictionary<string, string>();

        foreach (var currency in gameData.Currencies)
        {
            currencies.Add(currency.Key, currency.Value.CurrencyValue.Value.ToString());
        }

        researches = new Dictionary<string, bool>();

        foreach(var research in gameData.Researches)
        {
            researches.Add(research.Key, research.Value.IsBought.Value);
        }

        departments = new Dictionary<string, SaveDepartment>();

        foreach(var dep in gameData.Parameters.departments)
        {
            departments.Add(dep.Key, new SaveDepartment(dep.Value));
        }
    }
}

public class SaveDepartment
{
    public bool isBought;   
    public Dictionary<string, SaveBlock> saveBlocks;
    public Dictionary<string, SaveParameter> parameters;

    public SaveDepartment()
    {
        saveBlocks = new Dictionary<string, SaveBlock>();
        parameters = new Dictionary<string, SaveParameter>();
    }

    public SaveDepartment(RuntimeDepartmentData departmentData)
    {
        isBought = departmentData.IsBought.Value;
        parameters = new Dictionary<string, SaveParameter>();

        foreach(var parameter in departmentData.parameters)
        {
            if (parameter.Value.needToSave)
                parameters.Add(parameter.Key, parameter.Value.GetSaveData());
        }

        saveBlocks = new Dictionary<string, SaveBlock>();

        foreach(var block in departmentData.blocks)
        {
            saveBlocks.Add(block.Key, new SaveBlock(block.Value));
        }

    }
}

public class SaveBlock
{
    public bool isBought;
    public Dictionary<string, SaveParameter> parameters; 

    public SaveBlock()
    {
        parameters= new Dictionary<string, SaveParameter>();
    }

    public SaveBlock(RuntimeBlockData blockData)
    {
        isBought = blockData.IsBought.Value;
        parameters = new Dictionary<string, SaveParameter>();

        foreach(var parameter in blockData.parameters)
        {
            if (parameter.Value.needToSave)
                parameters.Add(parameter.Key, parameter.Value.GetSaveData());
        }
    }
}

public class SaveParameter
{
    public string value;
    public string type;

    public SaveParameter()
    {
        value = string.Empty;
        type = string.Empty;
    }
}