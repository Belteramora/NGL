
using System.Collections.Generic;
using Unity.VisualScripting;

public static class DictionaryExtensions
{
    public static GameParameter<T> Get<T>(this Dictionary<string, GameParameter> dict, string key)
    {
        return dict[key] as GameParameter<T>;
    }
}