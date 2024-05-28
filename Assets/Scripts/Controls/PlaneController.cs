using Cinemachine;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Tooltip("How much the throttle ramps up or down")]
    [SerializeField, Range(0f , 20f)] private float throttleIncrement;

	protected float throttle;
	protected float roll;
	protected float pitch;
	protected float yaw;

	[SerializeField] protected Plane playerPlane;

	[SerializeField] private HudInfo hudInfo;

	[SerializeField] protected CinemachineVirtualCamera mainVirtualCamera;
	[SerializeField] protected CinemachineVirtualCamera zoomVirtualCamera;

	protected virtual void HandleInputs()
	{
		roll = Input.GetAxis("Roll");
		pitch = Input.GetAxis("Pitch");
		yaw = Input.GetAxis("Yaw");

		throttle = playerPlane.throttle.Value;

		if (Input.GetKey(KeyCode.LeftShift)) throttle += throttleIncrement;
		else if (Input.GetKey(KeyCode.LeftControl)) throttle -= throttleIncrement;

		if (Input.GetKeyDown(KeyCode.G))
		{
			playerPlane.ChangeChassisStatus();
		}

		if (Input.GetMouseButton(0))
		{
			foreach (var weapon in playerPlane.weapons) 
			{
				if (weapon is Gun) weapon.FireCommand();
			}
		}

		if (Input.GetMouseButton(1))
		{
			zoomVirtualCamera.Priority = 11;
		}
		else
		{
			zoomVirtualCamera.Priority = 9;
		}
	}

	private void Update()
	{
		HandleInputs();

		playerPlane.ControllPlane(roll, pitch, yaw, throttle);

		hudInfo.UpdateHud(playerPlane);
	}

}