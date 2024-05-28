public class PlaneModulesCoeffs
{
    public float maneurability { get; private set; }
    public float thrust { get; private set; }

    public PlaneModulesCoeffs(float maneurability, float thrust)
    {
		this.maneurability = maneurability;
		this.thrust = thrust;
	}

	public static PlaneModulesCoeffs operator +(PlaneModulesCoeffs coeffs1, PlaneModulesCoeffs coeffs2)
	{
		float summedManeuverability = coeffs1.maneurability + coeffs2.maneurability;
		float summedThrust = coeffs1.thrust + coeffs2.thrust;
		return new PlaneModulesCoeffs(summedManeuverability, summedThrust);
	}
}