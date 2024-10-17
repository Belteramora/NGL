using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ExponentBlockUI : CellBlockUI
{

    private ExponentBlock exponentBlock;

    //TODO: не совсем понял как нормально можно провести тут зависимости (от базового к наследникам, т.к. тут желательно уточнять какой именно department)
    [Inject]
    public void Construct(ExponentBlock block, GameData gameData)
    {
        Debug.Log("M Block construct");

        this.exponentBlock = block;
        //base.BaseConstruct(block, gameData);
    }
}
