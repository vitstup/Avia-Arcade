using System.Collections.Generic;
using UnityEngine;

public class BulletPoolsHandler : MonoBehaviour
{
    private Dictionary<string, BulletPool> pools = new Dictionary<string, BulletPool>();

    public Bullet GetBullet(Bullet bullet)
    {
        if (pools.TryGetValue(bullet.name, out var pool))
        {
            return pool.GetElement();
        }
        else
        {
            return CreatePool(bullet).GetElement();
        }
    }

    private BulletPool CreatePool(Bullet bullet)
    {
		BulletPool pool = new BulletPool(bullet, 100, true, transform);
        pools.Add(bullet.name, pool);

        return pool;
    }
}