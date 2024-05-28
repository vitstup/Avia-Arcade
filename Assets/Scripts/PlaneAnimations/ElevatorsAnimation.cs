using UniRx;
using UnityEngine;

public class ElevatorsAnimation : BaseSurfaceControlAnimation
{
	[SerializeField] private Transform[] elevators;

	protected override FloatReactiveProperty GetReactiveProperty()
	{
		return plane.pitch;
	}

	protected override void ChangeSurface()
	{
		if (elevators != null)
		{
			float angle = plane.pitch.Value * maxAngle * GetInverseModifier();

			var direction = AxisConvertor.GetDirection(rotationAxis);

			foreach (Transform elevator in elevators)
			{
				elevator.localRotation = Quaternion.Euler(direction * angle);
			}
		}
	}

}