public interface IHitPointHolder
{
    int HitPoints { get; }
    void TakeDamage(int damage);
}
