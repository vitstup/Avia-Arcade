using TMPro;
using UnityEngine;

public class MarkerElementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectNameText;
    [SerializeField] private TextMeshProUGUI distanceText;

    public void SetupMarker(string objectName, int distance)
    {
		objectNameText.text = objectName;
		distanceText.text = distance.ToString() + "m";
	}

    public void ChangeDistance(int distance)
    {
		distanceText.text = distance.ToString() + "m";
	}
}