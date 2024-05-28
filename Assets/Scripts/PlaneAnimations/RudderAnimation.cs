using UniRx;
using UnityEngine;

public class RudderAnimation : BaseSurfaceControlAnimation
{
	[SerializeField] private Transform[] rudders;

	protected override FloatReactiveProperty GetReactiveProperty()
	{
		return plane.yaw;
	}

	protected override void ChangeSurface()
	{
		if (rudders != null)
		{
			float angle = plane.yaw.Value * maxAngle * GetInverseModifier();

			var direction = AxisConvertor.GetDirection(rotationAxis);

			foreach (Transform rudder in rudders)
			{
				rudder.localRotation = Quaternion.Euler(direction * angle);
			}
		}
	}

}