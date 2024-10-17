using BreakInfinity;
using Newtonsoft.Json;
using R3;
using System;
using static UnityEngine.Rendering.DebugUI;

public class GameParameter<T>: GameParameter
{
    [JsonIgnore]
    private T initValue;

    public T BaseValue
    {
        get => parameter.BaseValue.Value;
        set 
        { 
            parameter.BaseValue.Value = value;
            Update();
        }
    }

    [JsonIgnore]
    public T Value 
    {
        get => parameter.Value;
    }

    [JsonIgnore]
    private FunctionalReactiveProperty<T> parameter;

    public GameParameter(T value, bool needToSave = false)
    {
        initValue = value;
        this.needToSave = needToSave;
        parameter = new FunctionalReactiveProperty<T>(() => parameter.BaseValue.Value, initValue);

        type = typeof(T);
    }

    

    public void SetFunction(Func<T> resultFunction)
    {
        parameter = new FunctionalReactiveProperty<T>(resultFunction, initValue);

    }

    public override void Update()
    {
        parameter.Update();
    }

    public IDisposable SubscribeToModified(Action<T> action)
    {
        return parameter.ModifiedValue.Subscribe(action);
    }

    public IDisposable SubscribeToBase(Action<T> action)
    {
        return parameter.BaseValue.Subscribe(action);
    }

    public override SaveParameter GetSaveData()
    {
        return new SaveParameter { type = typeof(T).Name, value = parameter.BaseValue.ToString() };
    }
}

public abstract class GameParameter: IUpdatable
{
    [JsonIgnore]
    public bool needToSave;

    [JsonIgnore]
    public Type type;

    public abstract SaveParameter GetSaveData();

    public abstract void Update();
}