using UniRx;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour, IDieble
{
	[SerializeField] protected VehicleModule[] modules;
	[field: SerializeField, Range(1, 2500)] public int maxHP { get; private set; }
	public float currentHP { get; private set; }

	public BoolReactiveProperty died { get; private set; } = new BoolReactiveProperty(false);
	public DeathReason deathReason { get; private set; }

	private void Start()
	{
		SetLayerRecursively(gameObject, LayerMask.NameToLayer("Vehicle"));

		currentHP = maxHP;
	}

	private void SetLayerRecursively(GameObject obj, int layer)
	{
		obj.layer = layer;

		foreach (Transform child in obj.transform)
		{
			SetLayerRecursively(child.gameObject, layer);
		}
	}

	public void TakeDamage(RaycastHit hit, float damage, float piercing)
	{
		Debug.Log($"Vehicle part hitted: {hit.collider.name}, in {gameObject.name}");

		VehicleModule damagedModule = null;

		if (modules != null && modules.Length > 0)
		{
			for (int i = 0; i < modules.Length; i++)
			{
				if (modules[i].moduleCollider == hit.collider)
				{
					damagedModule = modules[i];
					break;
				}
			}
		}

		if (damagedModule != null)
		{
			damagedModule.Damage(damage, piercing);

			currentHP -= damagedModule.vehicleDamageCoeff * damage * damagedModule.GetPiercingModifier(piercing);
		}
		else currentHP -= damage;

		if (currentHP <= 0) Die(DeathReason.Killed);

	}

	public void Die(DeathReason reason)
	{
		if (died.Value) return;

		Debug.Log($"Vehicle {gameObject.name} destroyed");

		died.Value = true;

		deathReason = reason;

		AfterDeath();
	}

	protected virtual void AfterDeath()
	{
		var m = GetComponentInChildren<Marker>();
		if (m != null) m.RemoveMarker();
	}
}