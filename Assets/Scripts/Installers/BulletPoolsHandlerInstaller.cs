using UnityEngine;
using Zenject;

public class BulletPoolsHandlerInstaller : MonoInstaller
{
	[SerializeField] private BulletPoolsHandler bulletPoolsHandler;

	public override void InstallBindings()
	{
		Container.Bind<BulletPoolsHandler>().FromComponentInHierarchy(bulletPoolsHandler).AsSingle().NonLazy();
	}
}