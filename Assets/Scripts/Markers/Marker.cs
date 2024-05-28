using UnityEngine;

public class Marker : MonoBehaviour
{
    [field: SerializeField] public string markerName { get; private set; }

	private MarkersController markersController;

	private void Start()
	{
		markersController = FindObjectOfType<MarkersController>();

		markersController.AddMarker(this);
	}

	public void RemoveMarker()
	{
		markersController.RemoverMarker(this);
	}
}