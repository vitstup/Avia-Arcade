public interface IDieble : IDamagable
{
    public void Die(DeathReason reason = DeathReason.Unknown);
}