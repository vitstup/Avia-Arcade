using UnityEngine;

public static class DamagableLayers
{
    public static int damagableLayers
    {
        get => LayerMask.GetMask("Vehicle", "Ground", "Water", "Building");
	}

    public static int vehicleLayer
    {
        get => LayerMask.GetMask("Vehicle");
	}
}