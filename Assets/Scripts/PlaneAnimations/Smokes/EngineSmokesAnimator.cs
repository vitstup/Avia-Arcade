using System;
using UniRx;
using UnityEngine;

public class EngineSmokesAnimator : MonoBehaviour, IDisposable
{
	private Plane plane;

	private CompositeDisposable _disposables = new CompositeDisposable();

	private ParticleSystem[] smokes;

	private void Awake()
	{
		plane = GetComponentInParent<Plane>();
		smokes = GetComponentsInChildren<ParticleSystem>();
	}

	private void Start()
	{
		plane.throttle.Subscribe(v => { ChangeSmoke(); }).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}

	private void ChangeSmoke()
	{
		if (smokes == null || smokes.Length == 0) return;

		foreach (ParticleSystem p in smokes)
		{
			
		}

	}
}