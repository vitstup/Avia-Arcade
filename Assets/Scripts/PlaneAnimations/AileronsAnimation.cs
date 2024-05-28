using UniRx;
using UnityEngine;

public class AileronsAnimation : BaseSurfaceControlAnimation
{
	[SerializeField] private Transform alieronLeft;
	[SerializeField] private Transform alieronRight;

	private Vector3 alieronLeftBaseRotation;
	private Vector3 alieronRightBaseRotation;

	protected override void Start()
	{
		alieronLeftBaseRotation = alieronLeft.localRotation.eulerAngles;
		alieronRightBaseRotation = alieronRight.localRotation.eulerAngles;

		base.Start();
	}

	protected override FloatReactiveProperty GetReactiveProperty()
	{
		return plane.roll;
	}

	protected override void ChangeSurface()
	{
		float angle = plane.roll.Value * maxAngle * GetInverseModifier();

		var direction = AxisConvertor.GetTransformDirection(rotationAxis, transform);

		alieronLeft.localRotation = Quaternion.Euler(direction * angle + alieronLeftBaseRotation);
		alieronRight.localRotation = Quaternion.Euler(direction * angle + alieronRightBaseRotation);
	}
}