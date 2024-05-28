using UnityEngine;

public class PlaneEngineModule : PlaneModule
{
    [SerializeField, Range(0.2f, 1f)] private float maxDebaff;

	public override PlaneModulesCoeffs GetCoefs()
	{
		return new PlaneModulesCoeffs(0, maxDebaff * (1f - GetModuleIntegrity()));
	}
}