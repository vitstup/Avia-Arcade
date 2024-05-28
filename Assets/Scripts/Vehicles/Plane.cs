using System;
using UniRx;
using UnityEngine;

public class Plane : Vehicle
{
	public ReactiveCommand ChangeChassis { get; private set; } = new ReactiveCommand();

	[Header("Plane stats")]

	[Tooltip("Maximun engine power in horsepowers")]
	[SerializeField, Range(30f, 5000f)] private float enginePower;

	[Tooltip("Maximun power that mover can move per kilo in h/p.")]
	[SerializeField, Range(0.05f, 2f)] private float maxHpPerKilo;

	[Tooltip("Base Drag Of Plane, the lower -> the better")]
	[SerializeField, Range(0.85f, 2f)] private float dragCoefficient;

	[Tooltip("Maximum height of plane")]
	[SerializeField, Range(2500f, 20000f)] private float maxHeight;

	[Tooltip("Maximum height - this parametr is altitude level at wich engine will work less Efficinet untill fully collapse")]
	[SerializeField, Range(1000f, 10000f)] private float startsLosingEfficiencyAt;

	[Tooltip("Maximum height of plane")]
	[SerializeField, Range(5f, 30f)] private float maxEfficentAttackAngle;

	[SerializeField, Range(1f, 150f)] private float rollManeurabilityModifier;
	[SerializeField, Range(1f, 150f)] private float pitchManeurabilityModifier;
	[SerializeField, Range(1f, 150f)] private float yawManeurabilityModifier;

	[Tooltip("How much lift force this plane generates as it gains speed")]
	[SerializeField, Range(50, 2000f)] private float lift;

	[Tooltip("Theoretical Max Speed In Km/h")]
	[field: SerializeField, Range(50, 2000f)] public float theoreticalMaxSpeed { get; private set; }

	public Weapon[] weapons { get; private set; }

	public FloatReactiveProperty throttle { get; private set; } = new FloatReactiveProperty();
	public FloatReactiveProperty roll { get; private set; } = new FloatReactiveProperty();
	public FloatReactiveProperty pitch { get; private set; } = new FloatReactiveProperty();
	public FloatReactiveProperty yaw { get; private set; } = new FloatReactiveProperty();

	public float previousThrust { get; private set; }

	private float responsiveModifier => rb.mass * 0.1f;

	private float deathModifier => died.Value ? 0f : 1f;

	public Rigidbody rb { get; private set; }

	private CenterOfMass centerOfMass;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();

		centerOfMass = GetComponentInChildren<CenterOfMass>();
		if (centerOfMass != null) rb.centerOfMass = centerOfMass.transform.localPosition;
		else Debug.LogWarning($"There is no custom center of mass position for this plane {gameObject.name}");

		weapons = GetComponentsInChildren<Weapon>();
	}

	public void ControllPlane(float roll, float pitch, float yaw, float throttle)
	{
		if (died.Value)
		{
			this.throttle.Value = 0f;
			return;
		}

		this.roll.Value = Mathf.Clamp(roll, -1f, 1f);
		this.pitch.Value = Mathf.Clamp(pitch, -1f, 1f);
		this.yaw.Value = Mathf.Clamp(yaw, -1f, 1f);

		this.throttle.Value = Mathf.Clamp(throttle, 0, 100f);
	}

	public void ChangeChassisStatus()
	{
		ChangeChassis.Execute();
	}

	public void ForceSettingOfSpeed(float speed)
	{
		float power = speed / 3.6f * rb.mass;
		previousThrust = power;
		thrust = power;
	}

	private const float dragWhenSmashed = 6f;

	private const float hpToWt = 7.355f; // it is actually 735.5 w per one horse power but due to throttle is from 0 to 100, not 0 to 1 I decided to do this way
	private const float thrustModifier = 0.46f;
	private const float dragModifier = 0.5f;
	private const float inertinity = 0.05f;

	public float thrust { get; private set; }

	private void FixedUpdate()
	{
		if (died.Value && deathReason == DeathReason.Smashed) rb.drag = dragWhenSmashed;
		else rb.drag = dragModifier * GetAirDensity();

		var damageCoeffs = GetPlaneCoefsFromDamage();
		float thrustDamageCoeff = 1f - Math.Clamp(damageCoeffs.thrust, 0, 0.9f);
		float maneurabilityDamageCoeff = 1f - Math.Clamp(damageCoeffs.maneurability, 0, 0.9f);

		float currentMaxEnginePower = rb.mass * maxHpPerKilo * 100f;

		float currentEnginePower = Mathf.Clamp(enginePower * throttle.Value, 0, currentMaxEnginePower);

		thrust = currentEnginePower * hpToWt * GetEngineEfficiency() * thrustModifier * thrustDamageCoeff * deathModifier;

		var dif = (thrust - previousThrust) * Time.fixedDeltaTime * inertinity;

		var resultThrust = previousThrust + dif;

		previousThrust = resultThrust;

		rb.AddForce(transform.forward * resultThrust);

		rb.AddTorque(transform.up * yaw.Value * responsiveModifier * yawManeurabilityModifier * maneurabilityDamageCoeff);
		rb.AddTorque(transform.right * pitch.Value * responsiveModifier * pitchManeurabilityModifier * maneurabilityDamageCoeff);
		rb.AddTorque(-transform.forward * roll.Value * responsiveModifier * rollManeurabilityModifier * maneurabilityDamageCoeff);

		// Debug.Log("Roll: " + roll + ", Pitch: " + pitch + ", Yaw: " + yaw);
	}


	private void OnCollisionEnter(Collision collision)
	{
		var impulse = collision.impulse.magnitude;
		Smash(impulse);
	}

	private const float maxImpulseBeforeDeath = 10000f;
	private void Smash(float impulse)
	{
		Debug.Log($"Smash! {gameObject.name} Impulse: {impulse}");
		
		if (impulse >= maxImpulseBeforeDeath)
		{
			Die(DeathReason.Smashed);
		}
	}

	private float GetAirDensity()
	{
		const float LowestAirDensityAt = 20000f;

		const float LoswestAirDensity = 0.7f;
		const float HighestAirDensity = 1.3f;

		float percent = transform.position.y / LowestAirDensityAt;

		percent = Mathf.Clamp01(percent);


		float density = LoswestAirDensity + (HighestAirDensity - LoswestAirDensity) * (1f - percent);

		return density;
	}

	private float GetLiftAngleCoef()
	{
		var angle = transform.rotation.eulerAngles.x;

		float maximunAttackAngleForLifting = -70f;

		if (angle > 180f)
		{
			angle -= 360f;
		}

		if (angle >= 0) return 0f;
		else if (angle >= -maxEfficentAttackAngle)
		{
			return Mathf.Abs(angle / maxEfficentAttackAngle);
		}
		else if (angle >= maximunAttackAngleForLifting)
		{
			var percent = (angle + maxEfficentAttackAngle) / (maximunAttackAngleForLifting + maxEfficentAttackAngle);
			return 1f - percent;
		}
		else return 0f;
	}

	private float GetEngineEfficiency()
	{
		float height = transform.position.y;

		float startsLoseEffAtHeight = maxHeight - startsLosingEfficiencyAt;

		if (height < startsLoseEffAtHeight) return 1f;
		else
		{
			var res = 1f - (height - startsLoseEffAtHeight) / (maxHeight - startsLoseEffAtHeight);
			return Mathf.Clamp(res, 0f, 1f);
		}
	}

	public PlaneModulesCoeffs GetPlaneCoefsFromDamage()
	{
		PlaneModulesCoeffs coefs = new PlaneModulesCoeffs(0, 0);

		if (modules == null || modules.Length == 0) return coefs;

		foreach (var module in modules) 
		{
			if (module is PlaneModule)
			{
				var adding = (module as PlaneModule).GetCoefs();
				if (adding != null) coefs += adding;
			}
		}

		return coefs;
	}

	private void OnValidate()
	{
		if (startsLosingEfficiencyAt > maxHeight - 1000) startsLosingEfficiencyAt = maxHeight - 1000;
	}
}