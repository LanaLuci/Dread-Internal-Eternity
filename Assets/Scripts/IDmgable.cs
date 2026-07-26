using UnityEngine;

public interface IDmgable
{
    void TakeDmg(float amount);
    void GetHealed(float amount);
    void GetBurned(float amount, int stack);
}
