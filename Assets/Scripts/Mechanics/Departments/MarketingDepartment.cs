using BreakInfinity;

public class MarketingDepartment : DepartmentBase
{
    protected SellingBlock sBlock;

    public MarketingDepartment(GameData gameData): base(gameData)
    {
    }

    public override void Setup()
    {
        name = "MDep";

        sBlock = new SellingBlock(gameData, this);
        base.Setup();

        gainValue.SetFunction(() =>
        {
            return 1 + gameData.Parameters[name]["SBlock"].Get<BigDouble>("gain").Value;
        });
    }
}