using TMPro;
using UnityEngine;

public class HudInfo : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI ammoText;
	[SerializeField] private TextMeshProUGUI throttleText;
	[SerializeField] private TextMeshProUGUI speedText;
	[SerializeField] private TextMeshProUGUI altitudeText;

	public void UpdateHud(Plane plane)
	{
		ammoText.text = "Ammo: " + GetAmmoLeft(plane.weapons);
		throttleText.text = "Throttle: " + plane.throttle.Value.ToString("F0") + "%";
		speedText.text = "Speed: " + (plane.rb.velocity.magnitude * 3.6f).ToString("F0") + "km/h";
		altitudeText.text = "Altitude: " + plane.rb.transform.position.y.ToString("F0") + "m";
	}

	private int GetAmmoLeft(Weapon[] weapons)
	{
		int ammo = 0;
		if (weapons == null || weapons.Length == 0) return ammo;

		foreach (Weapon weapon in weapons)
		{
			if (weapon is Gun) ammo += (weapon as Gun).GetAmmoLeft();
		}

		return ammo;
	}
}