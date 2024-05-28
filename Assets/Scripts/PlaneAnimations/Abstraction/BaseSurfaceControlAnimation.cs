using System;
using UniRx;
using UnityEngine;

public abstract class BaseSurfaceControlAnimation : MonoBehaviour, IDisposable
{
	protected Plane plane;

	private CompositeDisposable _disposables = new CompositeDisposable();

	[SerializeField] private bool inversed;

	[SerializeField] protected Axis rotationAxis;

	[SerializeField, Range(1f, 90f)] protected float maxAngle;

	private void Awake()
	{
		plane = GetComponentInParent<Plane>();
	}

	protected virtual void Start()
	{
		GetReactiveProperty().Subscribe(v => { ChangeSurface(); }).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}

	protected abstract void ChangeSurface();

	protected abstract FloatReactiveProperty GetReactiveProperty();

	protected float GetInverseModifier() => inversed ? 1f : -1f;
}