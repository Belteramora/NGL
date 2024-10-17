using BreakInfinity;
using System.Collections.Generic;

public class ConfigData
{
    public Dictionary<string, CurrencySetup> currencySetups;
    public Dictionary<string, DepartmentData> departments;
    public Dictionary<string, ResearchSetup> researches;
}

public class CurrencySetup
{
    public string currencyName;
    public string currencyShortName;
}

public class DepartmentData
{
    public string displayName;
    public double cost;
    public bool isBought;

    public Dictionary<string, BlockData> blocks;
    public Dictionary<string, object> parameters;

    public DepartmentData()
    {
        parameters = new Dictionary<string, object>();
    }
}

public class BlockData
{
    public string displayName;
    public string displayLetter;
    public double cost;
    public bool isBought;

    public Dictionary<string, object> parameters;
}

public class ResearchSetup
{
    public string displayName;
    public string description;
    public string effectSign;
    public string attachedDepartment;
    public string attachedBlock;
    public double baseCost;
}
