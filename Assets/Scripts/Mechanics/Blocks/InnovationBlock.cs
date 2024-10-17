using System;
public class InnovationBlock : BlockBase
{
    //TODO: тут пусто, но пусть будет на всякий случай.
    public InnovationBlock(GameData gameData, DepartmentBase parentDepartment) : base(gameData, parentDepartment)
    {
    }

    public override void Setup()
    {
        name = "IBlock";
        base.Setup();
    }
}
