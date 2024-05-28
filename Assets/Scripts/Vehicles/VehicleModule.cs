using UnityEngine;

public class VehicleModule : MonoBehaviour
{
    [field: SerializeField] public string moduleName { get; private set; }
    public Collider moduleCollider { get; private set; }
    [field: SerializeField, Range(1, 1500)] public int maxHP { get; private set; }

	[Tooltip("Coefficent of damage to plane, for example if coeff == 1,5 and module taked 20 damage, then plane will get 30 damage")]
    [field: SerializeField, Range(0.1f, 200f)] public float vehicleDamageCoeff { get; private set; }

	[Tooltip("Armor, in milimetres")]
	[field: SerializeField, Range(0f, 100f)] public float armor { get; private set; }

	public float currentHP { get; private set; }

	private void Awake()
	{
		moduleCollider = GetComponent<Collider>();

		currentHP = maxHP;
	}

	public void Damage(float damage, float piercing)
	{
		currentHP -= damage * GetPiercingModifier(piercing);
		if (currentHP <= 0f) currentHP = 0f;
	}

	public virtual float GetPiercingModifier(float piercing)
	{
		return piercing > armor ? 1 : 0.1f;
	}

	public float GetModuleIntegrity()
	{
		return currentHP / (float)maxHP;
	}
}