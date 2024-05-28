using UnityEngine;

[RequireComponent(typeof(Plane))]
public class StartConditions : MonoBehaviour
{
    [Tooltip("Starting speed in km/h")]
    [SerializeField, Range(0, 800)] private int startingSpeed;

	[Tooltip("Starting throttle from 0 to 100")]
	[SerializeField, Range(0, 100f)] private float startingThrottle;

	private Plane plane;

	private void Awake()
	{
		plane = GetComponent<Plane>();
	}

	private void Start()
	{
		CompleteConditions();
		Deactivate();
	}

	private void CompleteConditions()
	{
		plane.ForceSettingOfSpeed(startingSpeed);

		plane.ControllPlane(0, 0, 0, startingThrottle);
	}

	private void Deactivate()
	{

	}
}