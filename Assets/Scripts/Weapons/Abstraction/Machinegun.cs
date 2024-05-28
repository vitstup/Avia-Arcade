using UnityEngine;

public abstract class Machinegun<T> : Gun where T : Bullet
{
	private float timeBetweenShots; 
	private float timeSinceLastShot;

	[field: SerializeField] public Belt<T> belt { get; private set; }

	[field: SerializeField] public int ammunition { get; private set; }

	public int ammoLeft { get; private set; }

	private int currentAmmoInBelt;

	private void Start()
	{
		ammoLeft = ammunition;
	}

	public override int GetAmmoLeft()
	{
		return ammoLeft;
	}

	public override void FireCommand()
	{
		TryToFire();
	}

	private void TryToFire()
	{
		timeBetweenShots = 60f / rpm;

		if (ammoLeft > 0 && timeSinceLastShot >= timeBetweenShots)
		{
			Fire(belt.Value[currentAmmoInBelt]);
			timeSinceLastShot = 0f;

			ChangeCurrentAmmoInBelt();

			ammoLeft--;
		}
	}

	private void Update()
	{
		timeSinceLastShot += Time.deltaTime;
	}

	private void ChangeCurrentAmmoInBelt()
	{
		currentAmmoInBelt++;

		if (currentAmmoInBelt >= belt.Value.Length) currentAmmoInBelt = 0;
	}
}