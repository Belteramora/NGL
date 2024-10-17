using BreakInfinity;

public class SellingBlock : CellsBlockBase
{
    public SellingBlock(GameData gameData, MarketingDepartment parentDepartment) : base(gameData, parentDepartment)
    {
    }

    public override void Setup()
    {
        name = "SBlock";
        base.Setup();

        cellBaseValue.SetFunction(() =>
        {

            return cellBaseValue.BaseValue + gameData.Researches["R3"].TryGetValue(0) + gameData.Researches["R4"].TryGetValue(0);
        });

        gainValue.SetFunction(() =>
        {
            return cellBaseValue.Value * cellsBought.Value * gameData.Parameters["MngDep"]["PBlock"].Get<BigDouble>(name).Value;
        });
    }
}