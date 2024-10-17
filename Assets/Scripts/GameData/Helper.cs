using BreakInfinity;
using System.Collections.Generic;

public static class Helper
{
    public static Dictionary<string, GameParameter> BindGenericGameParameters(Dictionary<string, object> objectParameters)
    {
        Dictionary<string, GameParameter> parameters = new Dictionary<string, GameParameter>();
        foreach (var objectParameter in objectParameters)
        {
            if (objectParameter.Value.GetType() == typeof(double))
                parameters[objectParameter.Key] = new GameParameter<BigDouble>((double)objectParameter.Value);
            else if (objectParameter.Value.GetType() == typeof(long))
                parameters[objectParameter.Key] = new GameParameter<long>((long)objectParameter.Value);
            else if (objectParameter.Value.GetType() == typeof(float))
                parameters[objectParameter.Key] = new GameParameter<BigDouble>((float)objectParameter.Value);
            else if (objectParameter.Value.GetType() == typeof(string))
                parameters[objectParameter.Key] = new GameParameter<string>(objectParameter.Value as string);
            else if (objectParameter.Value.GetType() == typeof(bool))
                parameters[objectParameter.Key] = new GameParameter<bool>((bool)objectParameter.Value);
        }

        return parameters;
    }

    public static void InjectSaveData(ref Dictionary<string, GameParameter> parameters, Dictionary<string, SaveParameter> saveParameters)
    {
        foreach (var param in saveParameters)
        {
            if (param.Value.type == "Int64")
                parameters[param.Key] = new GameParameter<long>(long.Parse(param.Value.value), true);
            else if (param.Value.type == "BigDouble")
                parameters[param.Key] = new GameParameter<BigDouble>(BigDouble.Parse(param.Value.value), true);
            else if (param.Value.type == "String")
                parameters[param.Key] = new GameParameter<string>(param.Value.value, true);
            else if (param.Value.type == "Boolean")
                parameters[param.Key] = new GameParameter<bool>(bool.Parse(param.Value.value), true);
        }
    }
}