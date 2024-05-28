using System.Collections.Generic;
using UnityEngine;

public class MonoPool<T> where T : MonoBehaviour
{
	private T prefab;
	private List<T> pool = new List<T>();

	public bool isAutoExpanded;

	public Transform parent;

	public MonoPool(T prefab, int count, bool isAutoExpanded, Transform parent = null)
	{
		this.prefab = prefab;
		this.isAutoExpanded = isAutoExpanded;
		this.parent = parent;
		CreatePool(count);
	}

	private void CreatePool(int count)
	{
		for (int i = 0; i < count; i++)
		{
			CreateObject();
		}
	}

	private T CreateObject(bool isActiveByDefault = false)
	{
		var obj = Object.Instantiate(prefab, parent);
		obj.gameObject.SetActive(isActiveByDefault);
		pool.Add(obj);
		return obj;
	}

	private bool HasFreeElement(out T element)
	{
		foreach (var obj in pool)
		{
			if (!obj.gameObject.activeSelf)
			{
				element = obj;
				return true;
			}
		}
		element = null;
		return false;
	}

	public T GetElement()
	{
		T result = null;
		if (HasFreeElement(out var element)) result = element;
		else if (!isAutoExpanded) throw new System.Exception("There is no free elements");
		else result = CreateObject();
		result.gameObject.SetActive(true);
		return result;
	}

	public List<T> GetFreeElements()
	{
		List<T> result = new List<T>();
		for (int i = 0; i < pool.Count; i++)
		{
			if (!pool[i].gameObject.activeInHierarchy) result.Add(pool[i]);
		}
		return result;
	}

	public List<T> GetBusyElements()
	{
		List<T> result = new List<T>();
		for (int i = 0; i < pool.Count; i++)
		{
			if (pool[i].gameObject.activeInHierarchy) result.Add(pool[i]);
		}
		return result;
	}
}