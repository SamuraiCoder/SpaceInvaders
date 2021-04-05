using Services;
using Services.Interfaces;
using UnityEngine;
using Zenject;

public class MainSceneInstallers : MonoInstaller
{
    public override void InstallBindings()
    {
    	Container.Bind<string>().FromInstance("Welcome to SpaceInvaders-Sybo! this msg tells Zenject is working well");
        Container.Bind<Greeter>().AsSingle().NonLazy();
        Container.Bind<IPauseService>().To<PauseService>().AsSingle().NonLazy();
        Container.Bind<IPositionService>().To<GameEntitiesPositionService>().AsSingle().NonLazy();
        Container.Bind<ISpawnerService>().To<SpaceInvadersSpawnerService>().AsSingle().NonLazy();
        Container.Bind(typeof(IEnemyMovementService), typeof(ITickable)).To<SpaceInvadersEnemyMovementService>().AsSingle().NonLazy();
        Container.Bind(typeof(IGameDirector), typeof(ITickable)).To<SpaceInvadersDirectorService>().AsSingle().NonLazy();
        Container.Bind<IScoreService>().To<ScoreManagerService>().AsSingle().NonLazy();
        Container.Bind<ILevelsService>().To<LevelsDefinitionService>().AsSingle().NonLazy();
    }

    private class Greeter
    {
        public Greeter(string message)
        {
            Debug.Log(message);
        }
    }
}