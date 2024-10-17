using BreakInfinity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;

public class RuntimeData : IUpdatable
{
    public Dictionary<string, RuntimeDepartmentData> departments;

    public RuntimeDepartmentData this[string key]
    {
        get => departments[key];
        set => departments[key] = value;
    }

    public RuntimeData()
    {
        departments = new Dictionary<string, RuntimeDepartmentData>();
    }

    public bool ContainsDepartment(string key)
    {
        return departments.ContainsKey(key);
    }

    public List<GameParameter> GetParametersByKeyMatch(string key)
    {
        List<GameParameter> parametersResult = new List<GameParameter>();

        foreach (var department in departments.Values)
        {
            parametersResult.AddRange(department.GetParametersByKeyMatch(key));
        }

        return parametersResult;
    }

    public void Update()
    {
        foreach(var department in departments.Values)
        {
            department.Update();
        }
    }

    public RuntimeBlockData GetBlock(string key)
    {
        var block = departments.Values.SelectMany(c => c.blocks).First(c => c.Key == key).Value;

        return block;
    }
}

public class RuntimeDepartmentData : IUpdatable
{
    public GameParameter<string> DisplayName { get; set; }
    public GameParameter<BigDouble> Cost { get; set; }

    public GameParameter<bool> IsBought { get; set; }

    public Dictionary<string, RuntimeBlockData> blocks;
    public Dictionary<string, GameParameter> parameters;

    public RuntimeBlockData this[string key]
    {
       get => blocks[key];
       set => blocks[key] = value;
    }

    public RuntimeDepartmentData(DepartmentData departmentData)
    {
        DisplayName = new GameParameter<string>(departmentData.displayName);
        Cost = new GameParameter<BigDouble>(departmentData.cost);
        IsBought = new GameParameter<bool>(departmentData.isBought, true);

        blocks = new Dictionary<string, RuntimeBlockData>();
        parameters = new Dictionary<string, GameParameter>();

        foreach (var block in departmentData.blocks)
        {
            var blockData = new RuntimeBlockData(block.Value);

            blocks[block.Key] = blockData;

        }

        parameters = Helper.BindGenericGameParameters(departmentData.parameters);
    }

    public GameParameter<T> Get<T>(string key)
    {
        return parameters[key] as GameParameter<T>;
    }

    public void Set(string key, GameParameter parameter)
    {
        parameters[key] = parameter;
    }

    public bool ContainsBlock(string key)
    {
        return blocks.ContainsKey(key);
    }

    public List<GameParameter> GetParametersByKeyMatch(string key)
    {
        List<GameParameter> parametersResult = new List<GameParameter>();

        foreach (var block in blocks.Values)
        {
            parametersResult.AddRange(block.GetParametersByKeyMatch(key));
        }

        return parametersResult;
    }

    public void Update()
    {
        DisplayName.Update();
        Cost.Update();
        IsBought.Update();

        foreach(var parameter in parameters.Values)
        {
            parameter.Update();
        }

        foreach(var block in blocks.Values)
        {
            block.Update();
        }
    }
}

public class RuntimeBlockData: IUpdatable
{
    public GameParameter<string> DisplayName { get; set; }
    public GameParameter<string> DisplayLetter { get; set; }
    public GameParameter<BigDouble> Cost { get; set; }

    public GameParameter<bool> IsBought { get; set; }

    public Dictionary<string, GameParameter> parameters;

    public GameParameter this[string key]
    {
        set => parameters[key] = value;
    }

    public RuntimeBlockData(BlockData blockData)
    {
        DisplayName = new GameParameter<string>(blockData.displayName);
        DisplayLetter = new GameParameter<string>(blockData.displayLetter);
        Cost = new GameParameter<BigDouble>(blockData.cost);
        IsBought = new GameParameter<bool>(blockData.isBought, true);

        parameters = Helper.BindGenericGameParameters(blockData.parameters);
    }

    public GameParameter<T> Get<T>(string key)
    {
        return parameters[key] as GameParameter<T>;
    }

    public bool ContainsParameter(string key)
    {
        return parameters.ContainsKey(key);
    }

    public List<GameParameter> GetParametersByKeyMatch(string key)
    {
        List<GameParameter> parametersResult = new List<GameParameter>();

        foreach (var parameter in parameters)
        {
            if(parameter.Key.Contains(key))
                parametersResult.Add(parameter.Value);
        }

        return parametersResult;
    }

    public Dictionary<string, GameParameter> GetParametersByKeyMatchWithKey(string key)
    {
        Dictionary<string, GameParameter> parametersResult = new Dictionary<string, GameParameter>();

        foreach (var parameter in parameters)
        {
            if (parameter.Key.Contains(key))
                parametersResult.Add(parameter.Key, parameter.Value);
        }

        return parametersResult;
    }

    public void Update()
    {
        DisplayName.Update();
        DisplayLetter.Update();
        Cost.Update();
        IsBought.Update();

        foreach(var parameter in parameters.Values)
        {
            parameter.Update();
        }
    }
}
