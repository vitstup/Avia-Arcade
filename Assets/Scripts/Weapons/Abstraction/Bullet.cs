using System.Collections;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [Tooltip("Starting speed in m/s")]
    [SerializeField, Range(50f, 1500f)] private float startingSpeed;
	[Tooltip("Mass in gramms")]
	[SerializeField, Range(1f, 1500f)] private float mass;
	[Tooltip("Damage")]
	[SerializeField, Range(5f, 1500f)] private float damage;
	[Tooltip("Damage")]
	[SerializeField, Range(3f, 200f)] private float piercing;
	[Tooltip("Visual length of bullet")]
	[SerializeField] private float lenght;

	private Rigidbody rb;

	private Vector3 previosPos;

	private IDamagable firedFrom;

	private void OnEnable()
	{
		if (rb == null)
		{
			rb = GetComponent<Rigidbody>();

			rb.mass = mass * 0.001f;

			rb.useGravity = true;

			rb.drag = 0.2f;
		}
	}

	public void Fired(Gun from)
	{
		firedFrom = from.GetComponentInParent<IDamagable>();

		transform.rotation = from.transform.rotation;
		transform.position = from.transform.position + from.transform.forward * lenght;

		previosPos = from.transform.position;

		rb.AddForce(transform.forward * startingSpeed, ForceMode.VelocityChange);

		StartCoroutine(LifeRoutine());

		StartCoroutine(HitRoutine());
	}

	private IEnumerator HitRoutine()
	{
		// Basically, as I tested after gameobject become inactive all coroutines stops automatically
		while (true)
		{
			yield return new WaitForSeconds(0.02f);
			// logic here

			var hits = Physics.RaycastAll(previosPos, transform.position - previosPos, Vector3.Distance(previosPos, transform.position), DamagableLayers.damagableLayers);
			if (hits != null && hits.Length > 0)
			{
				for (int i = 0; i < hits.Length; i++)
				{
					var damagable = hits[i].collider.GetComponentInParent<IDamagable>();
					if (damagable != null)
					{
						if (firedFrom != null && firedFrom == damagable) continue;

						damagable.TakeDamage(hits[i], damage, piercing);
					}
					
					Deactivate();
				}
			}


			previosPos = transform.position;
		}
	}

	private IEnumerator LifeRoutine()
	{
		yield return new WaitForSeconds(7f);

		Deactivate();
	}

	private void Deactivate()
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		gameObject.SetActive(false);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawRay(previosPos, transform.position - previosPos);
	}
}