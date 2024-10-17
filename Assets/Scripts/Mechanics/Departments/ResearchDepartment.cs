public class ResearchDepartment : DepartmentBase
{
    private InnovationBlock iBlock;
    public ResearchDepartment(GameData gameData) : base(gameData)
    {
    }

    public override void Setup()
    {
        name = "RDep";

        iBlock = new InnovationBlock(gameData, this);

        base.Setup();
    }
}
