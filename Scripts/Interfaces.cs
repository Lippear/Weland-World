using UnityEngine;

public interface ICharacter
{
    public void TakeDamage(float damage, float penetration);
    public void StopCharacter(bool isStuned,float seconds);
}
