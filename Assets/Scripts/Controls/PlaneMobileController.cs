using UnityEngine;

public class PlaneMobileController : PlaneController
{
	private bool zoomed;

	private void Start()
	{
		Input.gyro.enabled = true;
	}

	protected override void HandleInputs()
	{
		if (Mathf.Abs(Input.acceleration.z) <= 0.12) pitch = 0f;
		else pitch = Mathf.Clamp(-Input.acceleration.z * 2, -1f, 1f);

		// yaw

		// roll = Input.acceleration.x;
		// yaw = Input.acceleration.z;

		if (Mathf.Abs(Input.acceleration.x) <= 0.12) roll = 0f;
		else roll = Mathf.Clamp(Input.acceleration.x * 0.5f, -1f, 1f);



		// Debug.Log(Input.acceleration);
	}

	public void ChangeThrottle(float value)
	{
		throttle = value * 100f;
	}

	public void Shoot()
	{
		foreach (var weapon in playerPlane.weapons)
		{
			if (weapon is Gun) weapon.FireCommand();
		}
	}

	public void Zoom()
	{
		zoomed = !zoomed;

		if (zoomed) zoomVirtualCamera.Priority = 11;
		else zoomVirtualCamera.Priority = 9;
	}
}