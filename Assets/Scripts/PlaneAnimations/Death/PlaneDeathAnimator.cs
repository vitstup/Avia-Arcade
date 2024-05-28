using System;
using UniRx;
using UnityEngine;

public class PlaneDeathAnimator : MonoBehaviour, IDisposable
{
    private Plane plane;
    private ParticleSystem[] particles;

	private AudioSource audioSource;

    private CompositeDisposable disposables = new CompositeDisposable();

	private void Awake()
	{
		plane = GetComponentInParent<Plane>();
		particles = GetComponentsInChildren<ParticleSystem>();
		audioSource = GetComponentInChildren<AudioSource>();

		plane.died.Subscribe(_ => { ShowParticles(); }).AddTo(disposables);
	}

	public void Dispose()
	{
		disposables.Dispose();
	}

	private void ShowParticles()
	{
		if (!plane.died.Value) return;

		if (particles != null && particles.Length > 0)
		{
			foreach (var particle in particles)
			{
				particle.Play();
			}
		}

		if (audioSource != null)
		{
			audioSource.Play();
		}
	}
}