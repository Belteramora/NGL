using BreakInfinity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Localization.SmartFormat.Utilities;
using UnityEngine.Rendering;

public class JsonTest : MonoBehaviour
{
    string path = "C:\\Users\\Administrator\\Documents\\test.json";
    // Start is called before the first frame update
    void Start()
    {

        Write();
        Read();

        
    }

    public void Write()
    {
        SubCont cont = new SubCont();
        //cont.Sub = new GameParameter<BigDouble>(new BigDouble(1, 1000));
        cont.Sub = new GameParameter<double>(1000);
        var jsonString = JsonConvert.SerializeObject(cont, new GameParameterConverter());
        File.WriteAllText(path, jsonString);
    }

    public void Read()
    {
        var jsonString = File.ReadAllText(path);
        SubCont cont = JsonConvert.DeserializeObject<SubCont>(jsonString, new GameParameterConverter());
        Debug.Log(cont.Sub.Value);
    }

}


public class SubCont
{
    public GameParameter<double> Sub;
}

public class GameParameterConverter : JsonConverter<GameParameter>
{

    public override GameParameter ReadJson(JsonReader reader, Type objectType, GameParameter existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        // Read the JSON object into a dictionary
        //var value = serializer.Deserialize(reader);

        var dict = serializer.Deserialize(reader) as Dictionary<string, object>;
        foreach (var kvp in dict)
        {
            Debug.Log("key " + kvp.Key);
        }
        Debug.Log("ojb type is " + objectType);
        GameParameter parameter = new GameParameter<double>(0);

        //if (objectType == typeof(GameParameter<double>) || objectType == typeof(GameParameter<float>))
        //    parameter = new GameParameter<BigDouble>(reader.ReadAsDouble().Value);
        //else if(objectType == typeof(GameParameter<BigDouble>))
        //{
        //    var mantissa = reader.ReadAsDouble();
        //    var exponent = reader.ReadAsInt32();
        //    Debug.Log(reader.Value.GetType());

        //    Debug.Log(mantissa.Value + " " + exponent.Value);
        //    parameter = new GameParameter<BigDouble>(new BigDouble(mantissa.Value, exponent.Value));
        //}
        //else if (objectType == typeof(GameParameter<long>) || objectType == typeof(GameParameter<int>))
        //    parameter = new GameParameter<long>(reader.ReadAsInt32().Value);
        //else if (objectType == typeof(GameParameter<string>))
        //    parameter = new GameParameter<string>(reader.ReadAsString());
        //else if (objectType == typeof(GameParameter<bool>))
        //    parameter = new GameParameter<bool>(reader.ReadAsBoolean().Value);
        //else
        //    parameter = new GameParameter<BigDouble>((BigDouble)value);


        // Create a new Person object using the custom constructor
        return parameter;
    }

    public override void WriteJson(JsonWriter writer, GameParameter value, JsonSerializer serializer)
    {
        Debug.Log(serializer.GetType().Name);

        writer.WriteStartObject();
        writer.WritePropertyName("value");
        
        if (value.type == typeof(double))
        {
            var val = new BigDouble((value as GameParameter<double>).Value);
            writer.WriteStartObject();
            writer.WritePropertyName("Mantissa");
            writer.WriteValue(val.Mantissa);
            writer.WritePropertyName("Exponent");
            writer.WriteValue(val.Exponent);
            writer.WriteEndObject();
        }
        else if(value.type == typeof(BigDouble))
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Mantissa");
            writer.WriteValue((value as GameParameter<BigDouble>).BaseValue.Mantissa);
            writer.WritePropertyName("Exponent");
            writer.WriteValue((value as GameParameter<BigDouble>).BaseValue.Exponent);
            writer.WriteEndObject();
        }
        else if (value.type == typeof(long) || value.type == typeof(int))
            writer.WriteValue((value as GameParameter<long>).BaseValue);
        else if (value.type == typeof(string))
            writer.WriteValue((value as GameParameter<string>).BaseValue);
        else if (value.type == typeof(bool))
            writer.WriteValue((value as GameParameter<bool>).BaseValue);
        else
            writer.WriteValue((value as GameParameter<BigDouble>).BaseValue);

        writer.WriteEndObject();

    }
}
