using System.Collections.Generic;
using UnityEngine;

public class MarkersController : MonoBehaviour
{
	[SerializeField] private Camera mainCamera;

	[SerializeField] private Transform player;
	[SerializeField] private MarkerElementUI markerUiPrefab;
	[SerializeField] private Canvas canvas;

	private Dictionary<Marker, MarkerElementUI> markers = new Dictionary<Marker, MarkerElementUI>();

	public void AddMarker(Marker marker)
	{
		var UIElemet = Instantiate(markerUiPrefab, canvas.transform);

		UIElemet.SetupMarker(marker.markerName, (int)Vector3.Distance(player.position, marker.transform.position));

		markers.Add(marker, UIElemet);
	}

	public void RemoverMarker(Marker marker)
	{
		markers[marker].gameObject.SetActive(false);

		markers.Remove(marker);
	}

	private void Update()
	{
		var rect = markerUiPrefab.GetComponent<RectTransform>().rect;
		float minX = rect.width / 2;
		float maxX = Screen.width - minX;

		float minY = rect.height / 2;
		float maxY = Screen.height - minY;

		foreach (var marker in markers.Keys)
		{
			Vector2 pos = mainCamera.WorldToScreenPoint(marker.transform.position);
			pos.y += 50f;

			if (Vector3.Dot(marker.transform.position - transform.position, transform.forward) < 0)
			{
				if (pos.x < Screen.width / 2) pos.x = maxX;
				else pos.x = minX;
			}

			pos.x = Mathf.Clamp(pos.x, minX, maxX);
			pos.y = Mathf.Clamp(pos.y, minY, maxY);

			markers[marker].transform.position = pos;

			markers[marker].ChangeDistance((int)Vector3.Distance(player.position, marker.transform.position));
		}
	}
}