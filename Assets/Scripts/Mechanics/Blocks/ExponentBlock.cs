using BreakInfinity;
using Unity.VisualScripting;

public class ExponentBlock : CellsBlockBase
{
    public ExponentBlock(GameData gameData, ProductionDepartment parentDepartment) : base(gameData, parentDepartment)
    {
    }
    public override void Setup()
    {
        this.name = "EBlock";
        base.Setup();

        cellBaseValue.SetFunction(() =>
        {
            return cellBaseValue.BaseValue + gameData.Researches["R6"].TryGetValue(0);
        });

        gainValue.SetFunction(() =>
        {
            if (cellsBought.Value == 0)
                return 1;

            return cellBaseValue.Value * cellsBought.Value * gameData.Parameters["MngDep"]["PBlock"].Get<BigDouble>(name).Value;
        });
    }

}