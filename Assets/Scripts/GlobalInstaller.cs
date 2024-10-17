using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public TextAsset configData;
    public override void InstallBindings()
    {
        GeneralBind();
    }

    public void GeneralBind()
    {
        Container.BindInterfacesAndSelfTo<GameData>().AsSingle().WithArguments(configData);
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.Bind<IInitializable>().To<ResearchManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle();
    }
}
