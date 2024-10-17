using Newtonsoft.Json;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionalReactiveProperty<T>
{
    public ReactiveProperty<T> BaseValue { get; set; }

    public ReactiveProperty<T> ModifiedValue { get; private set; }
    public T Value {
        get
        {
            ModifiedValue.Value = resultFunction.Invoke();
            return ModifiedValue.Value;
        }

    }

    public Func<T> resultFunction;

    public FunctionalReactiveProperty(Func<T> resultFunction, T defaultValue = default)
    {
        this.resultFunction = resultFunction;

        BaseValue = new ReactiveProperty<T>(defaultValue);
        ModifiedValue = new ReactiveProperty<T>(defaultValue);
    }

    public void Update()
    {
        ModifiedValue.Value = resultFunction.Invoke();
    }

}
