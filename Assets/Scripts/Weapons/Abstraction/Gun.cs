using UnityEngine;
using Zenject;

public abstract class Gun : Weapon
{
	[field: SerializeField] public int rpm { get; private set; }

	[SerializeField] protected ParticleSystem particals;
	[SerializeField] protected AudioSource soundSource;

	[Inject] private BulletPoolsHandler bulletPoolsHandler;

	protected virtual void Fire(Bullet bullet)
	{
		var b = bulletPoolsHandler.GetBullet(bullet);
		b.Fired(this);

		FireEffects();
		FireSound();
	}

	protected virtual void FireEffects()
	{
		if (particals == null) return;

		particals.Play();
	}

	protected virtual void FireSound()
	{
		if (soundSource == null) return;

		soundSource.Play();
	}

	public abstract int GetAmmoLeft();

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, transform.forward);
		Gizmos.DrawWireSphere(transform.position + transform.forward, 0.1f);
	}
}