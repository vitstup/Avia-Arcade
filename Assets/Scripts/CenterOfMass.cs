using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;

		Gizmos.DrawWireSphere(transform.position, 0.5f);
	}
}