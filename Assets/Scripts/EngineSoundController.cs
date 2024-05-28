using System;
using UniRx;
using UnityEngine;

public class EngineSoundController : MonoBehaviour, IDisposable
{
	private CompositeDisposable disposables = new CompositeDisposable();

	private Plane plane;
	private AudioSource source;

	[SerializeField, Range(0.1f, 2f)] private float minPitch;
	[SerializeField, Range(0.2f, 2f)] private float maxPitch;

	private void Awake()
	{
		plane = GetComponentInParent<Plane>();
		source = GetComponent<AudioSource>();

		plane.throttle.Subscribe(_ => { ChangeSound(); }).AddTo(disposables);
	}

	public void Dispose()
	{
		disposables.Dispose();
	}

	private void ChangeSound()
	{
		if (plane.throttle.Value < 1f) source.mute = true;
		else
		{
			source.mute = false;
			float pitch = minPitch + (maxPitch - minPitch) * plane.throttle.Value * 0.01f;
			source.pitch = pitch;
		}
	}

	private void OnValidate()
	{
		if (minPitch > maxPitch - 0.1f) minPitch = maxPitch - 0.1f;
	}
}