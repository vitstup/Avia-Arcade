using System;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Plane), typeof(Animator))]
public class ChassisAnimation : MonoBehaviour, IDisposable
{
	private Plane plane;
	private Animator animator;

	private CompositeDisposable _disposables = new CompositeDisposable();

	private void Start()
	{
		animator = GetComponent<Animator>();
		plane = GetComponent<Plane>();

		plane.ChangeChassis.Subscribe(v => { ChangeChassisStatus(); }).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}

	private void ChangeChassisStatus()
	{
		animator.SetTrigger("ChassisOperation");
	}

	public void ChassisWasOpened()
	{
		animator.SetBool("IsChassisOpen", true);
	}

	public void ChassisWasClosed()
	{
		animator.SetBool("IsChassisOpen", false);
	}
}