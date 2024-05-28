using UnityEngine;

[System.Serializable]
public class Belt<T> where T : Bullet
{
    [field: SerializeField] public T[] Value { get; private set; }
}