using BreakInfinity;

public class ProductionDepartment : DepartmentBase
{
    protected AdditionBlock aBlock;
    protected MultiplicationBlock mBlock;
    protected ExponentBlock eBlock;
    public ProductionDepartment(GameData gameData): base(gameData)
    {
        
    }

    public override void Setup()
    {
        name = "PDep";

        aBlock = new AdditionBlock(gameData, this);
        mBlock = new MultiplicationBlock(gameData, this);
        eBlock = new ExponentBlock(gameData, this);

        base.Setup();

        gainValue.SetFunction(() =>
        {
            var ablockgain = gameData.Parameters[name]["ABlock"].Get<BigDouble>("gain");
            var mblockGain = gameData.Parameters[name]["MBlock"].Get<BigDouble>("gain");
            var eblockGain = gameData.Parameters[name]["EBlock"].Get<BigDouble>("gain");
            return BigDouble.Pow(ablockgain.Value * mblockGain.Value, eblockGain.Value);
        });
    }
}
