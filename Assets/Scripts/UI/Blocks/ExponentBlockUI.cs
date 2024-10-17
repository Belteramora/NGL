using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ExponentBlockUI : CellBlockUI
{

    private ExponentBlock exponentBlock;

    //TODO: �� ������ ����� ��� ��������� ����� �������� ��� ����������� (�� �������� � �����������, �.�. ��� ���������� �������� ����� ������ department)
    [Inject]
    public void Construct(ExponentBlock block, GameData gameData)
    {
        Debug.Log("M Block construct");

        this.exponentBlock = block;
        //base.BaseConstruct(block, gameData);
    }
}
