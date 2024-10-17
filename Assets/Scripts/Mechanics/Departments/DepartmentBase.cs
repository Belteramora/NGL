using BreakInfinity;
using System.Collections.Generic;

public abstract class DepartmentBase
{
    protected GameData gameData;

    public string name;
    public GameParameter<BigDouble> gainValue;
    public GameParameter<BigDouble> cost;
    public GameParameter<bool> isBought;

    public DepartmentBase(GameData gameData)
    {

        this.gameData = gameData;
        Setup();
    }

    public virtual void Setup()
    {
        var dep = gameData.Parameters[name];

        isBought = dep.IsBought;
        cost = dep.Cost;

        //TODO: убрать общий gain, так как на у всех департаментов есть
        gainValue = dep.Get<BigDouble>("gain");
    }
}
