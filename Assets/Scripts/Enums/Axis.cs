using UnityEngine;

public enum Axis
{
	X = 0,
	Y = 1,
	Z = 2,
}

public static class AxisConvertor
{
	public static Vector3 GetDirection(Axis axis)
	{
		Vector3 direction = Vector3.zero;
		if (axis == Axis.X) direction = Vector3.right;
		else if (axis == Axis.Y) direction = Vector3.up;
		else if (axis == Axis.Z) direction = Vector3.forward;

		return direction;
	}

	public static Vector3 GetTransformDirection(Axis axis, Transform transform)
	{
		Vector3 direction = Vector3.zero;
		if (axis == Axis.X) direction = transform.right;
		else if (axis == Axis.Y) direction = transform.up;
		else if (axis == Axis.Z) direction = transform.forward;

		return direction;
	}
}