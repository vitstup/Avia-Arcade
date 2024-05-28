using UnityEngine;

public class PlaneManeurabilityModule : PlaneModule
{
	[SerializeField, Range(0.2f, 1f)] private float maxDebaff;

	public override PlaneModulesCoeffs GetCoefs()
	{
		return new PlaneModulesCoeffs(maxDebaff * (1f - GetModuleIntegrity()), 0);
	}
}