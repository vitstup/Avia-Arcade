using UnityEngine;

public class PropellerAnimation : MonoBehaviour
{
	private Plane plane;

    [SerializeField] private Transform propella;
	[SerializeField] private Axis propellaRotationAxis;

	private const float maxSpeedPerFrame = 12f; // if speed will higher than this it may occure innacurate spinning

	private void Awake()
	{
		plane = GetComponentInParent<Plane>();
	}

	private void Update()
	{
		if (plane == null || propella == null) return;

		var propellaDirection = AxisConvertor.GetDirection(propellaRotationAxis);

		float throttleCoeff = plane.throttle.Value * 0.01f;

		throttleCoeff = Mathf.Clamp(throttleCoeff, 0, plane.GetPlaneCoefsFromDamage().thrust * 100f);

		if (plane.died.Value) throttleCoeff = 0;

		float speedFromEngine = maxSpeedPerFrame * throttleCoeff;
		float speedFromVelocity = maxSpeedPerFrame * (plane.rb.velocity.magnitude / (plane.theoreticalMaxSpeed / 3.6f));

		float speed = speedFromEngine * throttleCoeff + speedFromVelocity * (1f - throttleCoeff);

		speed = Mathf.Clamp(speed, 0, maxSpeedPerFrame);

		propella.Rotate(propellaDirection * speed);
	}
}