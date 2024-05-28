using UnityEngine;

public class BulletPool : MonoPool<Bullet>
{
	public BulletPool(Bullet prefab, int count, bool isAutoExpanded, Transform parent = null) : base(prefab, count, isAutoExpanded, parent)
	{

	}
}