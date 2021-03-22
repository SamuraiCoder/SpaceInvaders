using UnityEngine;
using Zenject;

public class MainSceneInstallers : MonoInstaller
{
    public override void InstallBindings()
    {
    	Container.Bind<string>().FromInstance("Welcome to SpaceInvaders-Sybo! this msg tells Zenject is working well");
        Container.Bind<Greeter>().AsSingle().NonLazy();
    }

    public class Greeter
    {
        public Greeter(string message)
        {
            Debug.Log(message);
        }
    }
}