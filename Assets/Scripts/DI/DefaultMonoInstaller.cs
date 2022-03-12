using UnityEngine;
using Zenject;

public class DefaultMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayerModal>().To<PlayerModal>().AsSingle();
        Container.Bind<IScoreModal>().To<ScoreModal>().AsSingle();
        Container.Bind<IStateModal>().To<StateModal>().AsSingle();
    }
}