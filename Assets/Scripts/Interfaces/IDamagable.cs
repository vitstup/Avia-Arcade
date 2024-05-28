using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(RaycastHit hit, float damage, float piercing);
}