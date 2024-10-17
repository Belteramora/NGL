public class ManagementDepartment : DepartmentBase
{
    private PlacementBlock pBlock;
    public ManagementDepartment(GameData gameData) : base(gameData)
    {
    }

    public override void Setup()
    {
        name = "MngDep";

        pBlock = new PlacementBlock(gameData, this);
        base.Setup();


    }
}
