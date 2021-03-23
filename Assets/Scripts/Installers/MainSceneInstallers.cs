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
        Container.Bind<IPositionService>().To<GameEntitiesPositionService>().AsSingle().NonLazy();
    }

    public class Greeter
    {
        public Greeter(string message)
        {
            Debug.Log(message);
        }
    }
}